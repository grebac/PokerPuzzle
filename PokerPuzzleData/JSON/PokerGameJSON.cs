using PokerPuzzleData.DTO;
using PokerPuzzleData.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PokerPuzzleData.JSON
{
    public class PokerGameJSON
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; }
        [JsonPropertyName("board")]
        public List<string> Board { get; set; }
        [JsonPropertyName("players")]
        public Dictionary<string, PlayerHandJSON> Players { get; set; }
        [JsonPropertyName("pots")]
        public List<StreetPotJSON> StreetPotJSONs { get; set; }

        #region JSONToDTO
        public List<CardsEnum> ParseBoard() {
            var board = new List<CardsEnum>();
            foreach (var card in Board)
            {
                board.Add(CardHelper.fromCodeToEnum(card));
            }
            return board;
        }

        public Dictionary<StreetEnum, int> ParseStreetPots() {
            Dictionary<StreetEnum, int> streetPots = new Dictionary<StreetEnum, int> { {StreetEnum.Preflop, 0 } };

            foreach (var streetPot in StreetPotJSONs) {
                StreetEnum street = StreetHelper.ParseStreet(streetPot.Street);
                streetPots.Add(street, streetPot.Size);
            }

            return streetPots;
        }

        public List<GameActionDTO> BuildGameActions()
        {
            List<GameActionDTO> actions = new List<GameActionDTO>();
            int index = 0;

            // Street names in JSON
            var streetsJSON = new List<string> {
                "p", // Preflop
                "f", // Flop
                "t", // Turn
                "r"  // River
            };

            // Get players by order
            var playersInOrder = Players
                .OrderBy(p => p.Value.Position)
                .ToList();

            // Track players who are still in the hand
            var activePlayerPositions = new HashSet<int>(playersInOrder.Select(p => p.Value.Position));

            foreach (var street in streetsJSON)
            {
                // 1️- Build action queues per player
                // It means spliting a string of action into a queue of single action
                // For example, "Kcc" means Check-Call-Call, is split into 'K' 'c' 'c'
                var actionQueues = new Dictionary<int, Queue<char>>();

                foreach (var (playerName, player) in playersInOrder)
                {
                    // There's a dictionnary for each Street. We hash each player's position with a Queue, either empty or filled if they took actions.
                    var bet = player.Bets.FirstOrDefault(b => b.Stage == street);

                    var queue = bet != null
                        ? new Queue<char>(bet.Actions)
                        : new Queue<char>();

                    actionQueues[player.Position] = queue;
                }

                // 2️- Consume queues round by round
                // Loop through each player in order for said street, and keep adding actions until every single action is consumed
                bool actionsRemaining = true;
                bool streetNotFullyFolded = false;

                while (actionsRemaining)
                {
                    actionsRemaining = false;

                    foreach (var (playerPosition, queue) in actionQueues)
                    {
                        if (queue.Count == 0) {
                            continue;
                        }

                        ActionTypeEnum action = ActionTypeHelper.ParseAction(queue.Dequeue().ToString());
                        if (action == ActionTypeEnum.Nothing) {
                            actionsRemaining = true;
                            continue;
                        }
                            

                        actions.Add(new GameActionDTO(
                            playerPosition,
                            StreetHelper.ParseStreet(street),
                            action,
                            index++
                        ));

                        // If the player folds, we remove him from the active player pool
                        if (action == ActionTypeEnum.Fold)
                        {
                            activePlayerPositions.Remove(playerPosition);
                            if (activePlayerPositions.Count <= 1)
                            {
                                actionsRemaining = false;
                                streetNotFullyFolded = false; // If every players but one folded, the game is ended on this street
                                break;
                            }
                        }

                        actionsRemaining = true;
                        streetNotFullyFolded = true;
                    }
                }

                // Add a "end of street" action
                if (streetNotFullyFolded) {
                    actions.Add(new GameActionDTO(
                        playerPosition: -1,
                        street: StreetHelper.ParseStreet(street),
                        action: ActionTypeEnum.StreetEnd,
                        orderIndex: index++
                    ));
                }
            }

            return actions;
        }
        #endregion
    }
}
