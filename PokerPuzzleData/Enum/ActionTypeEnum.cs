using PokerPuzzleData.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace PokerPuzzleData.Enum
{
    public enum ActionTypeEnum
    {
        [Description("pack://application:,,,/Ressources/Actions/fold.png")]
        Fold,
        [Description("pack://application:,,,/Ressources/Actions/call.png")]
        Call,
        [Description("pack://application:,,,/Ressources/Actions/bet.png")]
        Bet,
        [Description("pack://application:,,,/Ressources/Actions/check.png")]
        Check,
        [Description("pack://application:,,,/Ressources/Actions/raise.png")]
        Raise,
        Nothing,
        [Description("pack://application:,,,/Ressources/Actions/allin.png")]
        Allin
    }

    public static class ActionTypeHelper
    {
        public static ActionTypeEnum ParseAction(string c) {
            return c.ToLower() switch
            {
                "f" => ActionTypeEnum.Fold,
                "c" => ActionTypeEnum.Call,
                "b" => ActionTypeEnum.Bet,
                "k" => ActionTypeEnum.Check,
                "r" => ActionTypeEnum.Raise,
                "q" => ActionTypeEnum.Fold,
                "a" => ActionTypeEnum.Allin,
                _ => ActionTypeEnum.Nothing
            };
        }

        public static string ToImagePath(this ActionTypeEnum actionType)
        {
            var field = actionType.GetType().GetField(actionType.ToString());
            var attribute = field?.GetCustomAttribute<DescriptionAttribute>();

            return attribute?.Description ?? "";
        }
    }
}
