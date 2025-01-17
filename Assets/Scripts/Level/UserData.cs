using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UserData
{
    public static int lifes = 3, health;
    public static float coins;
    public static bool escudo, saltoDoble, vidaExtra, paracaidas;

    public static int nivelActual;

    private static int _nivelTerminado;
    private static bool _completoNivel1, _completoNivel2, _completoNivel3;
    private static bool _terminoPrimerVideo, _terminoLobby, _terminoLimbo, _terminoNivel1, _terminoTutorial, _terminoTutorial2, _terminoTutorial3, _terminoWorldTutorial1, _terminoWorldTutorial2, _terminoWorldTutorial3;
    private static bool _terminoVideoInicio, _terminoVideoVortex1, _terminoVideoVortex2, _terminoVideoVortex3;
    private static bool _piezaA_N1, _piezaA_N2, _piezaA_N3;
    private static bool _piezaB_N1, _piezaB_N2, _piezaB_N3;
    private static bool _piezaC_N1, _piezaC_N2, _piezaC_N3;
    private static bool _piezaD_N1, _piezaD_N2, _piezaD_N3;
    private static bool _usedPiezaA_N1, _usedPiezaA_N2, _usedPiezaA_N3;
    private static bool _usedPiezaB_N1, _usedPiezaB_N2, _usedPiezaB_N3;
    private static bool _usedPiezaC_N1, _usedPiezaC_N2, _usedPiezaC_N3;
    private static bool _usedPiezaD_N1, _usedPiezaD_N2, _usedPiezaD_N3;
    private static bool _playerGuide1, _playerGuide2, _playerGuide3, _playerGuide4, _playerGuide5, _playerGuide6;

    public static int nivelTerminado
    {
        get => _nivelTerminado;
        set
        {
            if (value > _nivelTerminado)
            {
                _nivelTerminado = value;
                PlayerPrefs.SetInt("nivelTerminado", value);

            }
        }
    }
    public static bool terminoPrimerVideo
    {
        get => _terminoPrimerVideo;
        set
        {
            _terminoPrimerVideo = value;
            if (value) PlayerPrefs.SetInt("terminoPrimerVideo", 1);
            nivelTerminado = 1;
        }
    }
    public static bool completoNivel1
    {
        get => _completoNivel1;
        set
        {
            _completoNivel1 = value;
            if (value) PlayerPrefs.SetInt("completoNivel1", 1);
            nivelTerminado = 2;
        }
    }

    public static bool completoNivel2
    {
        get => _completoNivel2;
        set
        {
            _completoNivel2 = value;
            if (value) PlayerPrefs.SetInt("completoNivel2", 1);
            nivelTerminado = 3;

        }
    }

    public static bool completoNivel3
    {
        get => _completoNivel3;
        set
        {
            _completoNivel3 = value;
            if (value) PlayerPrefs.SetInt("completoNivel3", 1);
            nivelTerminado = 4;
        }
    }

    public static bool terminoLobby
    {
        get => _terminoLobby;
        set
        {
            _terminoLobby = value;
            if (value) PlayerPrefs.SetInt("terminoLobby", 1);
        }
    }


    public static bool terminoLimbo
    {
        get => _terminoLimbo;
        set
        {
            _terminoLimbo = value;
            if (value) PlayerPrefs.SetInt("terminoLimbo", 1);
        }
    }

    public static bool terminoNivel1
    {
        get => _terminoNivel1;
        set
        {
            _terminoNivel1 = value;
            if (value) PlayerPrefs.SetInt("terminoNivel1", 1);
        }
    }

    public static bool terminoTutorial
    {
        get => _terminoTutorial;
        set
        {
            _terminoTutorial = value;
            if (value) PlayerPrefs.SetInt("terminoTutorial", 1);
        }
    }

    public static bool terminoTutorial2
    {
        get => _terminoTutorial2;
        set
        {
            _terminoTutorial2 = value;
            if (value) PlayerPrefs.SetInt("terminoTutorial2", 1);
        }
    }

    public static bool terminoTutorial3
    {
        get => _terminoTutorial3;
        set
        {
            _terminoTutorial3 = value;
            if (value) PlayerPrefs.SetInt("terminoTutorial3", 1);
        }
    }

    public static bool terminoWorldTutorial1
    {
        get => _terminoWorldTutorial1;
        set
        {
            _terminoWorldTutorial1 = value;
            if (value) PlayerPrefs.SetInt("terminoWorldTutorial1", 1);
        }
    }

    public static bool terminoWorldTutorial2
    {
        get => _terminoWorldTutorial2;
        set
        {
            _terminoWorldTutorial2 = value;
            if (value) PlayerPrefs.SetInt("terminoWorldTutorial2", 1);
        }
    }

    public static bool terminoWorldTutorial3
    {
        get => _terminoWorldTutorial3;
        set
        {
            _terminoWorldTutorial3 = value;
            if (value) PlayerPrefs.SetInt("terminoWorldTutorial3", 1);
        }
    }

    public static bool terminoVideoInicio
    {
        get => _terminoVideoInicio;
        set
        {
            _terminoVideoInicio = value;
            if (value) PlayerPrefs.SetInt("terminoVideoInicio", 1);
        }
    }

    public static bool terminoVideoVortex1
    {
        get => _terminoVideoVortex1;
        set
        {
            _terminoVideoVortex1 = value;
            if (value) PlayerPrefs.SetInt("terminoVideoVortex1", 1);
        }
    }

    public static bool terminoVideoVortex2
    {
        get => _terminoVideoVortex2;
        set
        {
            _terminoVideoVortex2 = value;
            if (value) PlayerPrefs.SetInt("terminoVideoVortex2", 1);
        }
    }

    public static bool terminoVideoVortex3
    {
        get => _terminoVideoVortex3;
        set
        {
            _terminoVideoVortex3 = value;
            if (value) PlayerPrefs.SetInt("terminoVideoVortex3", 1);
        }
    }

    public static bool piezaA_N1
    {
        get => _piezaA_N1;
        set
        {
            _piezaA_N1 = value;
            if (value) PlayerPrefs.SetInt("piezaA_N1", 1);
        }
    }

    public static bool piezaA_N2
    {
        get => _piezaA_N2;
        set
        {
            _piezaA_N2 = value;
            if (value) PlayerPrefs.SetInt("piezaA_N2", 1);
        }
    }

    public static bool piezaA_N3
    {
        get => _piezaA_N3;
        set
        {
            _piezaA_N3 = value;
            if (value) PlayerPrefs.SetInt("piezaA_N3", 1);
        }
    }

    public static bool piezaB_N1
    {
        get => _piezaB_N1;
        set
        {
            _piezaB_N1 = value;
            if (value) PlayerPrefs.SetInt("piezaB_N1", 1);
        }
    }

    public static bool piezaB_N2
    {
        get => _piezaB_N2;
        set
        {
            _piezaB_N2 = value;
            if (value) PlayerPrefs.SetInt("piezaB_N2", 1);
        }
    }

    public static bool piezaB_N3
    {
        get => _piezaB_N3;
        set
        {
            _piezaB_N3 = value;
            if (value) PlayerPrefs.SetInt("piezaB_N3", 1);
        }
    }

    public static bool piezaC_N1
    {
        get => _piezaC_N1;
        set
        {
            _piezaC_N1 = value;
            if (value) PlayerPrefs.SetInt("piezaC_N1", 1);
        }
    }

    public static bool piezaC_N2
    {
        get => _piezaC_N2;
        set
        {
            _piezaC_N2 = value;
            if (value) PlayerPrefs.SetInt("piezaC_N2", 1);
        }
    }

    public static bool piezaC_N3
    {
        get => _piezaC_N3;
        set
        {
            _piezaC_N3 = value;
            if (value) PlayerPrefs.SetInt("piezaC_N3", 1);
        }
    }

    public static bool piezaD_N1
    {
        get => _piezaD_N1;
        set
        {
            _piezaD_N1 = value;
            if (value) PlayerPrefs.SetInt("piezaD_N1", 1);
        }
    }

    public static bool piezaD_N2
    {
        get => _piezaD_N2;
        set
        {
            _piezaD_N2 = value;
            if (value) PlayerPrefs.SetInt("piezaD_N2", 1);
        }
    }

    public static bool piezaD_N3
    {
        get => _piezaD_N3;
        set
        {
            _piezaD_N3 = value;
            if (value) PlayerPrefs.SetInt("piezaD_N3", 1);
        }
    }

    public static bool usedPiezaA_N1
    {
        get => _usedPiezaA_N1;
        set
        {
            _usedPiezaA_N1 = value;
            if (value) PlayerPrefs.SetInt("usedPiezaA_N1", 1);
        }
    }

    public static bool usedPiezaA_N2
    {
        get => _usedPiezaA_N2;
        set
        {
            _usedPiezaA_N2 = value;
            if (value) PlayerPrefs.SetInt("usedPiezaA_N2", 1);
        }
    }

    public static bool usedPiezaA_N3
    {
        get => _usedPiezaA_N3;
        set
        {
            _usedPiezaA_N3 = value;
            if (value) PlayerPrefs.SetInt("usedPiezaA_N3", 1);
        }
    }

    public static bool usedPiezaB_N1
    {
        get => _usedPiezaB_N1;
        set
        {
            _usedPiezaB_N1 = value;
            if (value) PlayerPrefs.SetInt("usedPiezaB_N1", 1);
        }
    }

    public static bool usedPiezaB_N2
    {
        get => _usedPiezaB_N2;
        set
        {
            _usedPiezaB_N2 = value;
            if (value) PlayerPrefs.SetInt("usedPiezaB_N2", 1);
        }
    }

    public static bool usedPiezaB_N3
    {
        get => _usedPiezaB_N3;
        set
        {
            _usedPiezaB_N3 = value;
            if (value) PlayerPrefs.SetInt("usedPiezaB_N3", 1);
        }
    }

    public static bool usedPiezaC_N1
    {
        get => _usedPiezaC_N1;
        set
        {
            _usedPiezaC_N1 = value;
            if (value) PlayerPrefs.SetInt("usedPiezaC_N1", 1);
        }
    }

    public static bool usedPiezaC_N2
    {
        get => _usedPiezaC_N2;
        set
        {
            _usedPiezaC_N2 = value;
            if (value) PlayerPrefs.SetInt("usedPiezaC_N2", 1);
        }
    }

    public static bool usedPiezaC_N3
    {
        get => _usedPiezaC_N3;
        set
        {
            _usedPiezaC_N3 = value;
            if (value) PlayerPrefs.SetInt("usedPiezaC_N3", 1);
        }
    }

    public static bool usedPiezaD_N1
    {
        get => _usedPiezaD_N1;
        set
        {
            _usedPiezaD_N1 = value;
            if (value) PlayerPrefs.SetInt("usedPiezaD_N1", 1);
        }
    }

    public static bool usedPiezaD_N2
    {
        get => _usedPiezaD_N2;
        set
        {
            _usedPiezaD_N2 = value;
            if (value) PlayerPrefs.SetInt("usedPiezaD_N2", 1);
        }
    }

    public static bool usedPiezaD_N3
    {
        get => _usedPiezaD_N3;
        set
        {
            _usedPiezaD_N3 = value;
            if (value) PlayerPrefs.SetInt("usedPiezaD_N3", 1);
        }
    }

    public static bool playerGuide1
    {
        get => _playerGuide1;
        set
        {
            _playerGuide1 = value;
            if (value) PlayerPrefs.SetInt("playerGuide1", 1);
        }
    }

    public static bool playerGuide2
    {
        get => _playerGuide2;
        set
        {
            _playerGuide2 = value;
            if (value) PlayerPrefs.SetInt("playerGuide2", 1);
        }
    }

    public static bool playerGuide3
    {
        get => _playerGuide3;
        set
        {
            _playerGuide3 = value;
            if (value) PlayerPrefs.SetInt("playerGuide3", 1);
        }
    }

    public static bool playerGuide4
    {
        get => _playerGuide4;
        set
        {
            _playerGuide4 = value;
            if (value) PlayerPrefs.SetInt("playerGuide4", 1);
        }
    }

    public static bool playerGuide5
    {
        get => _playerGuide5;
        set
        {
            _playerGuide5 = value;
            if (value) PlayerPrefs.SetInt("playerGuide5", 1);
        }
    }

    public static bool playerGuide6
    {
        get => _playerGuide6;
        set
        {
            _playerGuide6 = value;
            if (value) PlayerPrefs.SetInt("playerGuide6", 1);
        }
    }

    static UserData()
    {
        LoadData();
    }

    private static void LoadData()
    {
        _terminoLobby = PlayerPrefs.GetInt("terminoLobby", 0) == 1;
        _terminoLimbo = PlayerPrefs.GetInt("terminoLimbo", 0) == 1;
        _terminoPrimerVideo = PlayerPrefs.GetInt("terminoPrimerVideo", 0) == 1;
        _terminoNivel1 = PlayerPrefs.GetInt("terminoNivel1", 0) == 1;
        _terminoTutorial = PlayerPrefs.GetInt("terminoTutorial", 0) == 1;
        _terminoTutorial2 = PlayerPrefs.GetInt("terminoTutorial2", 0) == 1;
        _terminoTutorial3 = PlayerPrefs.GetInt("terminoTutorial3", 0) == 1;
        _terminoWorldTutorial1 = PlayerPrefs.GetInt("terminoWorldTutorial1", 0) == 1;
        _terminoWorldTutorial2 = PlayerPrefs.GetInt("terminoWorldTutorial2", 0) == 1;
        _terminoWorldTutorial3 = PlayerPrefs.GetInt("terminoWorldTutorial3", 0) == 1;
        _terminoVideoInicio = PlayerPrefs.GetInt("terminoVideoInicio", 0) == 1;
        _terminoVideoVortex1 = PlayerPrefs.GetInt("terminoVideoVortex1", 0) == 1;
        _terminoVideoVortex2 = PlayerPrefs.GetInt("terminoVideoVortex2", 0) == 1;
        _terminoVideoVortex3 = PlayerPrefs.GetInt("terminoVideoVortex3", 0) == 1;
        _piezaA_N1 = PlayerPrefs.GetInt("piezaA_N1", 0) == 1;
        _piezaA_N2 = PlayerPrefs.GetInt("piezaA_N2", 0) == 1;
        _piezaA_N3 = PlayerPrefs.GetInt("piezaA_N3", 0) == 1;
        _piezaB_N1 = PlayerPrefs.GetInt("piezaB_N1", 0) == 1;
        _piezaB_N2 = PlayerPrefs.GetInt("piezaB_N2", 0) == 1;
        _piezaB_N3 = PlayerPrefs.GetInt("piezaB_N3", 0) == 1;
        _piezaC_N1 = PlayerPrefs.GetInt("piezaC_N1", 0) == 1;
        _piezaC_N2 = PlayerPrefs.GetInt("piezaC_N2", 0) == 1;
        _piezaC_N3 = PlayerPrefs.GetInt("piezaC_N3", 0) == 1;
        _piezaD_N1 = PlayerPrefs.GetInt("piezaD_N1", 0) == 1;
        _piezaD_N2 = PlayerPrefs.GetInt("piezaD_N2", 0) == 1;
        _piezaD_N3 = PlayerPrefs.GetInt("piezaD_N3", 0) == 1;
        _usedPiezaA_N1 = PlayerPrefs.GetInt("usedPiezaA_N1", 0) == 1;
        _usedPiezaA_N2 = PlayerPrefs.GetInt("usedPiezaA_N2", 0) == 1;
        _usedPiezaA_N3 = PlayerPrefs.GetInt("usedPiezaA_N3", 0) == 1;
        _usedPiezaB_N1 = PlayerPrefs.GetInt("usedPiezaB_N1", 0) == 1;
        _usedPiezaB_N2 = PlayerPrefs.GetInt("usedPiezaB_N2", 0) == 1;
        _usedPiezaB_N3 = PlayerPrefs.GetInt("usedPiezaB_N3", 0) == 1;
        _usedPiezaC_N1 = PlayerPrefs.GetInt("usedPiezaC_N1", 0) == 1;
        _usedPiezaC_N2 = PlayerPrefs.GetInt("usedPiezaC_N2", 0) == 1;
        _usedPiezaC_N3 = PlayerPrefs.GetInt("usedPiezaC_N3", 0) == 1;
        _usedPiezaD_N1 = PlayerPrefs.GetInt("usedPiezaD_N1", 0) == 1;
        _usedPiezaD_N2 = PlayerPrefs.GetInt("usedPiezaD_N2", 0) == 1;
        _usedPiezaD_N3 = PlayerPrefs.GetInt("usedPiezaD_N3", 0) == 1;
        _playerGuide1 = PlayerPrefs.GetInt("playerGuide1", 0) == 1;
        _playerGuide2 = PlayerPrefs.GetInt("playerGuide2", 0) == 1;
        _playerGuide3 = PlayerPrefs.GetInt("playerGuide3", 0) == 1;
        _playerGuide4 = PlayerPrefs.GetInt("playerGuide4", 0) == 1;
        _playerGuide5 = PlayerPrefs.GetInt("playerGuide5", 0) == 1;
        _playerGuide6 = PlayerPrefs.GetInt("playerGuide6", 0) == 1;

        _nivelTerminado = PlayerPrefs.GetInt("nivelTerminado", 0);
    }

    // Metodo para reiniciar piezas al morir
    public static void ResetPieces(int nivel)
    {
        switch (nivel)
        {
            case 1:
                if (!usedPiezaA_N1) piezaA_N1 = false;
                if (!usedPiezaB_N1) piezaB_N1 = false;
                if (!usedPiezaC_N1) piezaC_N1 = false;
                if (!usedPiezaD_N1) piezaD_N1 = false;
                break;
            case 2:
                if (!usedPiezaA_N2) piezaA_N2 = false;
                if (!usedPiezaB_N2) piezaB_N2 = false;
                if (!usedPiezaC_N2) piezaC_N2 = false;
                if (!usedPiezaD_N2) piezaD_N2 = false;
                break;
            case 3:
                if (!usedPiezaA_N3) piezaA_N3 = false;
                if (!usedPiezaB_N3) piezaB_N3 = false;
                if (!usedPiezaC_N3) piezaC_N3 = false;
                if (!usedPiezaD_N3) piezaD_N3 = false;
                break;
        }


        //Resetea de Puntos de partida
        ControlDatos._auxPoints = 0;

    }
    
}
