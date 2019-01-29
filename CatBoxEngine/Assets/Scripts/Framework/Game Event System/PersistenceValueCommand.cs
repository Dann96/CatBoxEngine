using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEventSystem
{
    public class PersistenceValueCommand : GameEventCommand
    {
        public string key;
        public string value;
        public GameEventCondition.StructCast structCast;
        public KeyInputType keyInputType;

        public override void InEditorUpdate(GameObject go){}

        protected override void ExecuteImmediately()
        {
            Dictionary<string, string> values = PlayerDataManager.instance.PlayerData.CurrentFile.gamePersistenceValues;

            if (!values.ContainsKey(key))
            {
                values.Add(key, value);
            }
            switch (structCast)
            {
                case GameEventCondition.StructCast.Bool:
                    values[key] = value;
                    break;
                case GameEventCondition.StructCast.Int:
                    int tempInt = int.Parse(value);
                    switch (keyInputType)
                    {
                        case KeyInputType.Increment:
                            tempInt++;
                            break;
                        case KeyInputType.Decrement:
                            tempInt--;
                            break;
                    }
                    value = tempInt.ToString();
                    values[key] = value;
                    break;
                case GameEventCondition.StructCast.Float:
                    float tempFloat = float.Parse(value);
                    switch (keyInputType)
                    {
                        case KeyInputType.Increment:
                            tempFloat++;
                            break;
                        case KeyInputType.Decrement:
                            tempFloat--;
                            break;
                    }
                    value = tempFloat.ToString();
                    values[key] = value;
                    break;
                case GameEventCondition.StructCast.String:
                    values[key] = value;
                    break;
            }
            PlayerDataManager.instance.PlayerData.CurrentFile.gamePersistenceValues = values;
        }

        public enum KeyInputType
        {
            Increment = 0,
            Decrement = 1,
            SolidValue = 2
        }
    }
}
