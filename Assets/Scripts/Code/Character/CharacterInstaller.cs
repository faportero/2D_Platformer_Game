using InputFolder;
using System;
using System.Collections;
using UnityEngine;

namespace Character
{
    //Clase para la configuración Inicial que se usa para el character, se establece qué tipo de Input se va a usar
    public class CharacterInstaller : MonoBehaviour
    {
        public int _lvl;
        [SerializeField] private bool _useJoystick;
        [SerializeField] private bool _useTapOnScreen;
        [SerializeField] private bool _useGyro;
        [SerializeField] private bool _useIA;
        [SerializeField] private bool _startWithGame;
        [SerializeField] private Joystick _joystick;
        [SerializeField] private GasButtonOnUI _gasButton;
        public CharacterMediator _character;
        [SerializeField] private DialButtonOnUI _joystickDial;

        //Se Configura el character con el Input obtenido
        private void Awake()
        {
            Application.targetFrameRate = 60;
            _character.Configure(GetInput());
            if (_startWithGame)
            {
                GamePause(false);
                _character.enabled = true;
            }
            else
            {
                GamePause(true);
                _character.enabled = false;
            }
        }
        //Obtiene el tipo de Input que se va a usar
        private InputInterface GetInput()
        {
            //if (Application.isMobilePlatform)
            //{
            //    _useJoystick = true;
            //    _useGyro = false;
            //    _useIA = false;
            //    _useTapOnScreen = false;
            //}
            if (_useJoystick)
                return new JoystickInputAdapter(_joystick, _character, _gasButton, _joystickDial);
            Destroy(_joystick.gameObject);
            if (_useTapOnScreen)
                return new TapOnScreenInputAdapter(_character, _gasButton, _joystickDial);
            if (_useIA)
                return new AIInputAdapter(_character);
            //Destroy(_gasButton.gameObject);
            return new UnityInputAdapter(_character, _gasButton, _joystickDial);
        }

        public void Configure()
        {
            _character.Configure(GetInput());
        }

        public void GamePause(bool pause)
        {
            StartCoroutine(SetTimeScale(pause));
        }
        IEnumerator SetTimeScale(bool pause)
        {
            yield return new WaitForSecondsRealtime(.1f);
            if (pause)
                Time.timeScale = 0.0f;
            else
                Time.timeScale = 1.0f;
        }
        public bool IsIAInput()
        {
            if (_useIA)
                return true;
            return false;
        }
    }
}