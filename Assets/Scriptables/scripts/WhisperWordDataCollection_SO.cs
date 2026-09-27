using System.Collections.Generic;
using UnityEngine;

namespace WhisperPrototype
{
    [CreateAssetMenu(fileName = "all_whispers", menuName = "Whisper_Word/WhisperWordDataCollection_SO")]
    public class WhisperWordDataCollection_SO : DataCollection<WhisperWordData_SO>
    {
        private Dictionary<string, WhisperWordData_SO> dictWhisper;
        void Awake()
        {
            dictWhisper = new Dictionary<string, WhisperWordData_SO>();
            foreach(var whisper in DataList)
            {
                dictWhisper[whisper.GetKey()] = whisper;
            }
        }
        public override WhisperWordData_SO GetDataByKey(string key) => dictWhisper[key];
        public WhisperWordData_SO GetWhisperByNote(int[] notes)
        {
            return null;
        }
    }
}
