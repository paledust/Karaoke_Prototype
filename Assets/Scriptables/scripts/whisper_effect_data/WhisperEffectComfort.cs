using System;
using DG.Tweening;
using UnityEngine;
using WhisperPrototype.Spirit;

namespace WhisperPrototype
{   
    public class WhisperComfort:WhisperEffect
    {
        protected GameObject zonePrefab;
        protected GameObject hugSpiritPrefab;
        protected SpiritHugger spiritHugger;
        public WhisperComfort(float duration, GameObject zonePrefab, GameObject hugSpiritPrefab):base(duration)
        {
            this.hugSpiritPrefab = hugSpiritPrefab;
            this.zonePrefab = zonePrefab;
        }
        internal override void InitializeWhisper(WhisperHandler caster, Vector3 castPosition, float radius)
        {
            //Create Spirit
            base.InitializeWhisper(caster, castPosition, radius);
            spiritHugger = GameObject.Instantiate(hugSpiritPrefab).GetComponent<SpiritHugger>();
            spiritHugger.transform.position = caster.transform.position + Vector3.forward * 0.5f;
            spiritHugger.Init(caster.gameObject);

            //Create Zone
            CreateZone(zonePrefab, castPosition, radius, duration);
        }
    }
    [CreateAssetMenu(fileName = "WhisperEffectComfort", menuName = "Whisper_Word/Effect/WhisperEffectComfort")]
    public class WhisperEffectComfort : WhisperEffectData_SO
    {
        [SerializeField] private GameObject hugSpiritPrefab;
        
        public override WhisperEffect GetWhisperEffect()
        {
            return new WhisperComfort(duration, whisper_zone_prefab, hugSpiritPrefab);
        }
    }
}
