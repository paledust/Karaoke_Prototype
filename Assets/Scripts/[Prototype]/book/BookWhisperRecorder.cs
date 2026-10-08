using System.Collections;
using System.Threading.Tasks;
using AudioAnalysis;
using DG.Tweening;
using SimpleAudioSystem;
using UnityEngine;

namespace WhisperPrototype.Book
{
    public class BookWhisperRecorder : MonoBehaviour
    {
        [SerializeField] private ParticleSystem vfxWhispering;
        [SerializeField] private SpriteRenderer whisperRenderer;
        [SerializeField] private PerRenderWhisper perRenderWhisper;
        [SerializeField] private GameObject hintTxt;

        [Header("Maie Mimic")]
        [SerializeField] private Animator maieAnime;

        [Header("Unlock Whisper Audio")]
        [SerializeField] private AudioData_SO sfxWhisperAppear;

        private NoteRecorder playerNoteRecorder;
        private WhisperWordData_SO checkingWhisper;

        private const string ANIM_SINGING_BOOL = "IsSinging";

        void Start()
        {
            PlayerWhisperingEvent.E_OnPlayerStartWhispering += StartWhisperingHandler;
            PlayerWhisperingEvent.E_OnPlayerStopWhispering += StopWhisperingHandler;
         }
        void OnDestroy()
        {
            PlayerWhisperingEvent.E_OnPlayerStartWhispering -= StartWhisperingHandler;
            PlayerWhisperingEvent.E_OnPlayerStopWhispering -= StopWhisperingHandler;
        }
        public void CleanUp()
        {
            vfxWhispering.gameObject.SetActive(false);
            whisperRenderer.gameObject.SetActive(false);
            hintTxt.SetActive(false);
            if(playerNoteRecorder!=null)
            {
                playerNoteRecorder.OnNoteMatchingWhisper -= NoteMatchingHandler;
                playerNoteRecorder = null;
            }
        }
        public void PrepareForNewWhisper(WhisperWordData_SO newWhisper)
        {
            vfxWhispering.gameObject.SetActive(true);
            whisperRenderer.gameObject.SetActive(true);
            hintTxt.SetActive(true);
            checkingWhisper = newWhisper;
            whisperRenderer.sprite = newWhisper.GetIcon();
            perRenderWhisper.dissolveStart = 0;
        }

        void StartWhisperingHandler(AudioAnalyzer playerAnalyzer)
        {
            playerNoteRecorder = playerAnalyzer.GetComponent<NoteRecorder>();
            playerNoteRecorder.OnNoteMatchingWhisper += NoteMatchingHandler;
            maieAnime.SetBool(ANIM_SINGING_BOOL, true);
        }
        void StopWhisperingHandler()
        {
            playerNoteRecorder.OnNoteMatchingWhisper -= NoteMatchingHandler;
            playerNoteRecorder = null;
            maieAnime.SetBool(ANIM_SINGING_BOOL, false);
        }
        void NoteMatchingHandler(WhisperWordData_SO whisper)
        {
            if(whisper != null && whisper == checkingWhisper)
            {
                perRenderWhisper.dissolveStart = 0;
                AudioManager.Instance.PlaySFX(sfxWhisperAppear.name, 1);
                DOTween.To(()=>perRenderWhisper.dissolveStart, x=>perRenderWhisper.dissolveStart = x, 1, 3f)
                        .SetEase(Ease.OutQuad)
                        .OnComplete(()=>StartCoroutine(coroutineFinishLearning()));
            }
        }
        IEnumerator coroutineFinishLearning()
        {
            WhisperingManager.UnlockWhisper(checkingWhisper);
            yield return new WaitForSeconds(0.5f);
            yield return GetComponent<BookOfWhisper>().CloseBook();
            yield return new WaitForSeconds(0.5f);
            PlayerWhisperingEvent.Call_OnPlayerEndLeanredNewWhisper(checkingWhisper);
            checkingWhisper = null;
        }
    }
}