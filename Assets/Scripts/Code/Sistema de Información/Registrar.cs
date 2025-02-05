using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using System;
using Unity.VisualScripting;
using static UnityEngine.Rendering.DebugUI;
using Cysharp.Threading.Tasks;

public class Registrar : MonoBehaviour
{
    //variables para registro e ingreso
    [Header("Entradas de texto para Registro")]
    public TMP_InputField _nombre;
    public TMP_InputField _correo;
    public TMP_InputField _telefono;
    public TextMeshProUGUI _mensaje;
    public TMP_InputField _password;
    [Header("Paneles")]
    public GameObject _panelEspera;
    public GameObject _panelLogin, _panelMenu, _panelRegistro, _panelRestablecerPassword, _panelEditarDatos;
    private bool _isLoggin = false;
    private static int _panelReturn;
    public static string auxPassword;
    public static string _genero;
    private string _AuxMensaje;

    public GameObject player;
    public AudioSettings audioSettings;
    public ControlDatos controlesPersonaje;
    private bool _isLogginRegistro;
    string textoBienvenida;

    [SerializeField]UI_Page _panelMenuController;
    [SerializeField]UI_MenuController UI_MenuController;
    private async void Start()
    {
        // if (_panelEspera != null && gameObject.name != "Panel Registro") _panelEspera.SetActive(true);
        print("Usuario Actual: " + ControlDatos._loginCorreo);
        if (gameObject.name == "Panel Login")
        {
            _panelEspera.SetActive(true);
            if (PlayerPrefs.HasKey("User_Email") && PlayerPrefs.HasKey("User_Password"))
            {
                string savedEmail = PlayerPrefs.GetString("User_Email");
                string savedPassword = PlayerPrefs.GetString("User_Password");
                await AutoLogin(savedEmail, savedPassword);
            }
            else
            {
                //  if (_panelEspera != null && gameObject.name != "Panel Registro") _panelEspera.SetActive(false);
                _panelEspera.SetActive(false);
                InitializeInputFields();

            }
        }
        else
        {
            InitializeInputFields();

        }
        // if (_panelEspera != null) _panelEspera.SetActive(true);
    }

    private async UniTask AutoLogin(string email, string password)
    {
        ControlDatos._loginCorreo = email;
        ControlDatos._loginPassword = password;
        await ControlDatos.ObtenerUsuario();
        if (ControlDatos._Estado != 0)
        {
            // Manejo de error: mostrar el panel de login
            Debug.LogError("Error en el login automático: " + ControlDatos._Mensaje);
            _panelEspera.SetActive(false);
            //_panelLogin.SetActive(true);
            //UI_MenuController.PushPage(_panelLogin.GetComponent<UI_Page>());

        }
        else
        {
            // Login exitoso: continuar con la aplicación
            Debug.Log("Login automático exitoso. Usuario logueado: " + ControlDatos._loginCorreo);
            if (UI_MenuController != null) UI_MenuController.PopAllPages();
            if (UI_MenuController != null && _panelMenuController != null) UI_MenuController.PushPage(_panelMenuController);
            _panelEspera.SetActive(false);
        }
    }
    private void InitializeInputFields()
    {
        if (_nombre != null) _nombre.contentType = TMP_InputField.ContentType.Standard;
        if (_correo != null) _correo.contentType = TMP_InputField.ContentType.Standard;
        if (_telefono != null) _telefono.contentType = TMP_InputField.ContentType.Standard;
        if (_password != null) _password.contentType = TMP_InputField.ContentType.Password;
        if (_nombre != null) _nombre.ActivateInputField();
        if (_correo != null) _correo.ActivateInputField();
        if (_telefono != null) _telefono.ActivateInputField();
        if (_password != null) _password.ActivateInputField();

        if (ControlDatos.menu)
        {
            gameObject.SetActive(false);
            UI_MenuController.PushPage(_panelMenuController);
            ControlDatos.menu = false;
        }
        else
        {
            if (name == "Panel Login")
            {
                _panelReturn = 2;
                _isLoggin = false;
                TextMeshProUGUI[] textMeshPros = FindObjectsOfType<TextMeshProUGUI>();
                foreach (var tmp in textMeshPros)
                {
                    if (tmp.gameObject.name == "MensajeLogin")
                    {
                        _mensaje = tmp.GetComponent<TextMeshProUGUI>();
                        break;
                    }
                }
                TMP_InputField[] inputFields = FindObjectsOfType<TMP_InputField>();
                foreach (var tmp in inputFields)
                {
                    if (tmp.gameObject.name == "InputUserLogin")
                        _correo = tmp.GetComponent<TMP_InputField>();
                    if (tmp.gameObject.name == "InputPasswordLogin")
                        _password = tmp.GetComponent<TMP_InputField>();
                }
            }
            if (name == "Panel Registro")
            {
                _panelReturn = 1;
                _isLoggin = false;
                TextMeshProUGUI[] textMeshPros = FindObjectsOfType<TextMeshProUGUI>();
                foreach (var tmp in textMeshPros)
                {
                    if (tmp.gameObject.name == "Mensaje")
                    {
                        _mensaje = tmp.GetComponent<TextMeshProUGUI>();
                        break;
                    }
                }
                TMP_InputField[] inputFields = FindObjectsOfType<TMP_InputField>();
                foreach (var tmp in inputFields)
                {
                    if (tmp.gameObject.name == "InputUser")
                        _correo = tmp.GetComponent<TMP_InputField>();
                    if (tmp.gameObject.name == "InputPassword")
                        _password = tmp.GetComponent<TMP_InputField>();
                    if (tmp.gameObject.name == "InputName")
                        _nombre = tmp.GetComponent<TMP_InputField>();
                    if (tmp.gameObject.name == "InputCelular")
                        _telefono = tmp.GetComponent<TMP_InputField>();
                }
            }
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            // Borrar credenciales guardadas
            PlayerPrefs.DeleteKey("User_Email");
            PlayerPrefs.DeleteKey("User_Password");

            // Verificar si las credenciales fueron borradas
            bool emailDeleted = !PlayerPrefs.HasKey("User_Email");
            bool passwordDeleted = !PlayerPrefs.HasKey("User_Password");

            // Imprimir el estado de la operación
            Debug.Log("Credenciales borradas: " + (emailDeleted && passwordDeleted));
            Debug.Log("Usuario logueado actual: " + ControlDatos._loginCorreo);
            Debug.Log("Usuario guardado actual: " + (PlayerPrefs.HasKey("User_Email") ? PlayerPrefs.GetString("User_Email") : "Ninguno"));
        }
    }

    public void Registro(GameObject panel)
    {
        _panelReturn = 1;
        if (IsValidEmail(_correo.text.ToString()) && _nombre.text != "" && _password.text != "" && _password.text.Length >= 5)
        {
            ControlDatos._Correo = _correo.text;
            ControlDatos._Password = _password.text;
            ControlDatos._Nombre = _nombre.text;
            ControlDatos._IsDemo = 0;
            ControlDatos._IsScorm = 1;
            if(_telefono) ControlDatos._NumeroCelular = _telefono.text;
            panel.SetActive(true);
            ControlDatos.CrearUsuario(); // Ojo
            ControlDatos._Mensaje = "";
            ControlDatos._Estado = 1;
            auxPassword = _password.text;
            _mensaje.text = "";
            _AuxMensaje = "";

            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetString("User_Email", _correo.text);
            PlayerPrefs.SetString("User_Password", _password.text);
            PlayerPrefs.Save();

            gameObject.SetActive(false);
            return;
        }
        else if(_nombre.text == "")
        {
            _AuxMensaje = "Usuario no válido";
            _mensaje.text = _AuxMensaje;
        }
        else if (_correo.text == "" || !IsValidEmail(_correo.text.ToString()))
        {
            _AuxMensaje = "Correo electrónico no válido";
            _mensaje.text = _AuxMensaje;
        }
        else if (_password.text.Length < 5)
        {
            _AuxMensaje = "La contraseña debe tener al menos 5 caracteres";
            _mensaje.text = _AuxMensaje;
        }
        else
        {
            _AuxMensaje = "Campos vacíos o inválidos";
            _mensaje.text = _AuxMensaje;
        }
    }
    public void IngresandoRegistro()
    {
        ControlDatos._Mensaje = "";
        ControlDatos._Estado = 1;
        if (_mensaje.text != "Campos vacíos o inválidos" && _mensaje.text != "Las contraseñas no coinciden" && _mensaje.text != "La contraseña debe tener al menos 6 caracteres" && gameObject.activeSelf)
        {
            StartCoroutine(TiempoEsperaRegistro());
            _mensaje.text = "";
            _AuxMensaje = "";
        }
    }
    IEnumerator TiempoEsperaRegistro()
    {
        ControlDatos._Mensaje = "";
        ControlDatos._Estado = 1;
        _mensaje.text = "";
        _AuxMensaje = "";
        //Debug.Log("El mensaje ANTES DE... es: " + ControlDatos._Mensaje);
        yield return new WaitWhile(() => ControlDatos._Mensaje == "");
        if (ControlDatos._Mensaje == "Usuario registrado y agregado al juego" && ControlDatos._Estado == 0) _isLogginRegistro = true;
        else _isLogginRegistro = false;
        _mensaje.text = ControlDatos._Mensaje;
        //Debug.Log("El mensaje es: " + _mensaje.text);
        //Debug.Log("El usuario está registrado? " + _isLogginRegistro);
        if (_isLogginRegistro)
        {
            //ControlDatos._loginCorreo = ControlDatos._Correo;
            //ControlDatos._loginPassword = ControlDatos._Password;
            //ControlDatos._loginPassword = "Pass_123"; // Para login sin contraseña. Registro solo para enviar información.
            //ControlDatos.ObtenerUsuario();
            StartCoroutine(TiempoParaCrearInventarioRegistro());
            StartCoroutine(RestablecerAuxMensaje());
            _panelReturn = 0;
        }
        else
        {
            ControlDatos._Estado = 1;
            _panelEspera.SetActive(false);
            if (_panelReturn == 2)
            {
                _panelLogin.SetActive(true);
            }
            if (_panelReturn == 1)
            {
                _panelRegistro.SetActive(true);
            }
        }
    }
    public void Login(GameObject panel)
    {
        _panelReturn = 2;
        if (_correo.text != "" && _password.text != "")
        {
            ControlDatos._loginCorreo = _correo.text;
            ControlDatos._loginPassword = _password.text;
            //print("Correo: " + ControlDatos._loginCorreo + ". Password: " + ControlDatos._loginPassword);
            ControlDatos.ObtenerUsuario(); // Ojo
            //ControlDatos._loginCorreo = "";
            //ControlDatos._loginPassword = "";
            ControlDatos._Mensaje = "";
            ControlDatos._Estado = 1;
            auxPassword = _password.text;
            _mensaje.text = "";
            _AuxMensaje = "";

            PlayerPrefs.SetString("User_Email", _correo.text);
            PlayerPrefs.SetString("User_Password", _password.text);
            PlayerPrefs.Save();

            panel.SetActive(true);
            gameObject.SetActive(false);
            //if (PlayerPrefs.GetString("User_Name") == "")
            //{
            //    panel.SetActive(true);
            //    gameObject.SetActive(false);
            //}
            //else
            //{
            //    StartCoroutine(IngresandoLoginCorrutina(panel));
            //}
            //print("User_Name" + PlayerPrefs.GetString("User_Name") + " contraseña: " + PlayerPrefs.GetString("User_Name"));
        }
        else
        {
            _AuxMensaje = "Datos inválidos";
            _mensaje.text = _AuxMensaje;
            ControlDatos._Mensaje = "";
            ControlDatos._Estado = 1;
        }
        //IngresandoLogin();
    }  
    //---------------------> LoginAutomático
    IEnumerator IngresandoLoginCorrutina(GameObject panel)
    {
        gameObject.GetComponent<Animator>().enabled = false;
        panel.SetActive(true);
        IngresandoLogin();
        yield return new WaitForSecondsRealtime(1.5f);
        //transform.GetChild(1).GetComponent<MovePanelMovil>().PosOriginal(); // Ojoooo
        //yield return new WaitForSecondsRealtime(1f);
        //transform.GetChild(1).GetComponent<MovePanelMovil>().PosOriginal();
        //gameObject.GetComponent<Animator>().enabled = false;
        yield return new WaitForSecondsRealtime(2.5f);
        //panel.SetActive(true);

        if (ControlDatos._Mensaje != "No encontrado")
        {
            gameObject.SetActive(false);
        }
        else
        {
            //_panelLogin.SetActive(true);
            //_panelLogin.transform.GetChild(1).GetComponent<MovePanelMovil>().PosOriginal();
            //_panelLogin.GetComponent<Animator>().enabled = true;
            PlayerPrefs.SetString("User_Name", "");
            PlayerPrefs.SetString("User_Password", "");
            //print("GameObject: " + gameObject.name);
            panel.SetActive(false);
            gameObject.SetActive(true);
            //_panelLogin.SetActive(true);
            //transform.GetChild(1).GetComponent<MovePanelMovil>().PosOriginal(); //Ojooo
            gameObject.GetComponent<Animator>().enabled = true;
        }
    }
    public void IngresandoLogin()
    {
        ControlDatos._Mensaje = "";
        ControlDatos._Estado = 1;
        if (_AuxMensaje != "Datos inválidos" && gameObject.activeSelf)
        {
            StartCoroutine(TiempoEsperaLogin());
            _mensaje.text = "";
            _AuxMensaje = "";
        }
    }
    IEnumerator TiempoEsperaLogin()
    {
        ControlDatos._Mensaje = "";
        ControlDatos._Estado = 1;
        _mensaje.text = "";
        _AuxMensaje = "";
        yield return new WaitWhile(() => ControlDatos._Mensaje == "");
        if (ControlDatos._Mensaje == "Satisfactorio" && ControlDatos._Estado == 0) _isLoggin = true;
        else _isLoggin = false;
        _mensaje.text = ControlDatos._Mensaje;
        //Debug.Log("El mensaje es: " + _mensaje.text);
        //Debug.Log("El usuario está logeado? " + _isLoggin);
        if (_isLoggin)
        {
            //PlayerPrefs.SetString("User_Name", ControlDatos._Nombre);
            //PlayerPrefs.SetString("User_Password",auxPassword);
            StartCoroutine(TiempoParaLeerInventario());
            StartCoroutine(TiempoParaAplicarInventario());
            StartCoroutine(RestablecerAuxMensaje());
            _panelReturn = 0;
        }
        else
        {
            _panelEspera.SetActive(false);
            if (_panelReturn == 2)
            {
                _panelLogin.SetActive(true);
            }
            if (_panelReturn == 1)
            {
                 _panelRegistro.SetActive(true);
                //_panelRegistro.transform.GetChild(0).gameObject.SetActive(true);
                //_panelRegistro.GetComponent<AnimPanelRegistro>().animatorPanel.SetBool("Registro", true); //Ojo
            }
        }
    }
    public void EditarDatos()
    {
        if (IsValidEmail(_correo.text.ToString()) && _nombre.text != "" && _password.text != "" && _password.text.Length >= 6)
        {
            ControlDatos._Correo = _correo.text;
            ControlDatos._Password = _password.text;
            ControlDatos._Nombre = _nombre.text;
            ControlDatos._IsDemo = 0;
            ControlDatos._IsScorm = 1;
            ControlDatos._NumeroCelular = 0995949431.ToString();
            //ControlDatos.EditarUsuario(); // Ojo
            ControlDatos._Mensaje = "";
            ControlDatos._Estado = 1;
            auxPassword = _password.text;
            _AuxMensaje = "";
            StartCoroutine(RestablecerAuxMensaje());
            StartCoroutine(EnviarCorreoEditarUsuario());
            return;
        }
        else if (_nombre.text == "")
        {
            _AuxMensaje = "Usuario no válido";
            _mensaje.text = _AuxMensaje;
        }
        else if (_correo.text == "" || !IsValidEmail(_correo.text.ToString()))
        {
            _AuxMensaje = "Correo electrónico no válido";
            _mensaje.text = _AuxMensaje;
        }
        else if (_password.text.Length < 6)
        {
            _AuxMensaje = "La contraseña debe tener al menos 6 caracteres";
            _mensaje.text = _AuxMensaje;
            StartCoroutine(RestablecerAuxMensaje());
        }
    }
    bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
    IEnumerator EnviarCorreoEditarUsuario()
    {
        yield return new WaitForSecondsRealtime(.2f);
        if (_mensaje.text == "correcto")
        {
            string asunto = "Usuario modificaco con éxito";
            string correo = "¡Hola! <b><i>" + ControlDatos._Nombre + "</i></b>. Tus datos han sido actualizados exitosamente.";
            //ControlDatos.EnviarCorreo(ControlDatos._Correo, asunto, correo); //Ojo
            PlayerPrefs.SetString("User_Name", ControlDatos._Nombre);
            PlayerPrefs.SetString("User_Password", auxPassword);
        }
    }
    IEnumerator RestablecerAuxMensaje()
    {
        yield return new WaitForSecondsRealtime(3);
        ControlDatos._Mensaje = "";
        _AuxMensaje = "";
    }
    public void RestablecerContraseña()
    {
        if (IsValidEmail(_correo.text.ToString()) && _nombre.text != "" && _correo.text != "")
        {
            ControlDatos._loginCorreo = _nombre.text;
            ControlDatos._Correo = _correo.text;
            ControlDatos._Nombre = _nombre.text;
            //ControlDatos.RecuperarContraseña(_nombre.text, _correo.text); //Ojo
            ControlDatos._loginCorreo = "";
            ControlDatos._loginPassword = "";
            ControlDatos._Mensaje = "";
            ControlDatos._Estado = 1;
            _mensaje.text = "";
            _AuxMensaje = "";
            PlayerPrefs.SetString("User_Name", "");
            PlayerPrefs.SetString("User_Password", "");
            StartCoroutine(RestablecerAuxMensaje());
        }
        else if (_correo.text == "" || !IsValidEmail(_correo.text.ToString()))
        {
            _AuxMensaje = "Correo electrónico no válido";
            _mensaje.text = _AuxMensaje;
        }
        else
        {
            _AuxMensaje = "Datos inválidos";
            _mensaje.text = _AuxMensaje;
            ControlDatos._Mensaje = "";
            ControlDatos._Estado = 1;
            StartCoroutine(RestablecerAuxMensaje());
        }
    }
    public void LlenarDatosParaEditar()
    {
        if (!_nombre) _nombre = transform.GetChild(0).GetChild(0).GetComponentInChildren<TMP_InputField>();
        if (!_correo) _correo = transform.GetChild(0).GetChild(1).GetComponentInChildren<TMP_InputField>();
        if (!_password) _password = transform.GetChild(0).GetChild(2).GetComponent<TMP_InputField>();
        if (!_mensaje) _mensaje = transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>();
        transform.GetChild(0).GetChild(0).GetComponent<InputField>().text = ControlDatos._Nombre;
        transform.GetChild(0).GetChild(1).GetComponent<InputField>().text = ControlDatos._Correo;
        transform.GetChild(0).GetChild(2).GetComponent<InputField>().text = auxPassword;
        _nombre.text = ControlDatos._Nombre;
        //_correo.text = ControlDatos._Correo;
        //_password.text = ControlDatos._Password;
    }
    public void VaciarMensaje()
    {
        ControlDatos._Mensaje = "";
        ControlDatos._Estado = 1;
        _mensaje.text = "";
        _AuxMensaje = "";
    }
    IEnumerator TiempoParaAplicarInventario()
    {
        //StartCoroutine(TiempoParaLeerInventario());
        //if (audioSettings) audioSettings.LoadInternalSavedSettings(); //Ojo
        yield return new WaitForSeconds(.5f);
        _panelEspera.SetActive(false);
        //_panelMenu.SetActive(true);
        UI_MenuController.PushPage(_panelMenuController);
    }
    IEnumerator TiempoParaLeerInventario()
    {
        //ControlDatos.ObtenerObjetoInventarioPorUsuarioId(); // Ojo
        yield return new WaitForSeconds(.25f);
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (audioSettings) audioSettings.LoadSavedSettings();
        ControlDatos.ObtenerObjetoInventarioPorUsuarioToken();
    }
    IEnumerator TiempoParaAplicarInventarioRegistro()
    {
        yield return new WaitForSeconds(.25f);
        _panelEspera.SetActive(false);
        //_panelMenu.SetActive(true); 
        UI_MenuController.PushPage(_panelMenuController);

        if (audioSettings) audioSettings.LoadSavedSettings();
        ControlDatos._listaObjetosInventario = new List<Vector2>
        {
            new Vector2(1, 0),
            new Vector2(2, 0),
            new Vector2(3, 0),
            new Vector2(4, 0)
        };
        ControlDatos.CrearEditarObjetoInventario();
        ControlDatos.ObtenerRanking();
    }
    IEnumerator TiempoParaCrearInventarioRegistro()
    {
        string asunto = "Programa de Transformación Ecológica de Veolia";
        string correo = "¡Hola! <b><i>" + ControlDatos._Nombre + "</i></b>. Bienvenido(a) a esta aventura donde, junto a Sam, ayudarás a salvar Guayaquil del caos provocado por la llegada de seres extraños. " +
            "Tu misión será rescatar a tus amigas las plantas de la contaminación, cuidar el agua y restaurar la ciudad. Suerte en tu camino para convertirte en un miembro más de la Liga de Renovadores. ¡Vamos!";
        //ControlDatos.EnviarCorreo(ControlDatos._Correo, asunto, correo); // Ojo
        ControlDatos._Mensaje = "";
        ControlDatos._Estado = 1;
        yield return new WaitWhile(() => !_isLogginRegistro);
        StartCoroutine(TiempoParaAplicarInventarioRegistro());

    }
    public void ShowPassword(TMP_InputField input)
    {
        input.contentType = TMP_InputField.ContentType.Standard;
        input.ForceLabelUpdate();
    }
    public void HidePassword(TMP_InputField input)
    {
        input.contentType = TMP_InputField.ContentType.Password;
        input.ForceLabelUpdate();
    }
    public void CambiarMensajeTexto()
    {
        _mensaje.text = "";
        _AuxMensaje = "";
        ControlDatos._Mensaje = "";
        _mensaje.text = "";
    }
}
