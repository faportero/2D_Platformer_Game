using Assets.Scripts.Modelos;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
//NOTA: PARA USAR UNITASK: Cysharp.Threading.Tasks; visitar el github: https://github.com/Cysharp/UniTask/tree/master
public partial class ControlDatos : MonoBehaviour
{
    [SerializeField] private TimerHud _timer;
    //private static string rutaPrincipal = "http://192.168.100.91:4001/juegosdm_api";
    private static string rutaPrincipal = "https://juegosdm.retmeup.com";
    private static string nombreJuego = "Undo";
    //AudioSettings
    public static int _audioAmbiental = 50, _audioEfectos = 50;

    //Inputs para ingresar
    public static string _loginCorreo;
    public static string _loginPassword;

    //Inputs para crear
    public static string _Id;
    public static string _Nombre;
    public static string _Correo;
    public static string _Password;
    public static string _NumeroCelular;
    public static string _IdentificadorUsuario; // Es para ver el número de cédula o pasaporte
    public static string _tokenUsuario; // Es para ver el número de cédula o pasaporte
    public static string _nombreGame;
    public static int _IsDemo;
    public static int _IsScorm;
    public static int _currentNivel;
    public static DateTime _startTime, _currentTime;
    //Mensajes con JsonCompleto
    public static string _Mensaje = "";
    public static int _Estado;

    //Inputs de inventario
    public static List<Vector2> _listaObjetosInventario;
    public static int _IdObjetoInventario;
    public static int _CantidadObjetoInventario;

    //Datos de Guardado de nivel
    public static int _bestPoints, _points, _coins, _nivel, _auxPoints;
    public static bool _isWinnerLvl1, _isWinnerLvl2, _isWinnerLvl3, _isWinnerLvl4, _isWinnerLvl5, _isDiestro;

    public static bool isNewGame = true;
    public static bool menu = false;

    public static Respuesta_Modelo<RankingModel> respuestaRanking;

    public static string _asuntoCorreo, _textoCorreo;

    //Para obtener un usuario (En caso de Login)
    public static async UniTask ObtenerUsuario()
    {
        await AsyncObtenerUsuario(_loginCorreo, _loginPassword);
    }

    private static async UniTask AsyncObtenerUsuario(string username, string password)
    {
        LoginUsuarioModel usuarioModelEnviar = new LoginUsuarioModel()
        {
            correo = username,
            contrasena = password
        };
        var json = JsonUtility.ToJson(usuarioModelEnviar);
        UnityWebRequest response = new UnityWebRequest(rutaPrincipal + "/usuario/login/" + nombreJuego, "POST");
        SetUnityWebRequest(response, json);
        await UniTask.Delay(100);
        await response.SendWebRequest();
        if (response.result == UnityWebRequest.Result.Success)
        {
            var jsonString = response.downloadHandler.text;
            Respuesta_Modelo<UsuarioModel> respuestaModelo = JsonUtility.FromJson<Respuesta_Modelo<UsuarioModel>>(jsonString);
            _Estado = respuestaModelo.response.error;
            _Mensaje = respuestaModelo.response.msg;
            if (respuestaModelo.response.info != "")
            {
                _tokenUsuario = respuestaModelo.response.info;
                DecodeJWT(_tokenUsuario);
                _startTime = DateTime.Now;
                //Debug.Log("Fecha y Hora Inicial: " + _startTime.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            _Id = respuestaModelo.response.id;
            //_Correo = username;
            _Password = password;
            _IsDemo = respuestaModelo.response.demo;
            _IsScorm = respuestaModelo.response.scorm;
            //Debug.Log("JSON Obtenido de Login: " + jsonString + ". Token: " +_tokenUsuario);
        }
    }

    public static void CrearUsuario()
    {
        RegisterUserModel a = new RegisterUserModel()
        {
            nombre = _Nombre,
            correo = _Correo,
            contrasena = _Password,
            telefono = _NumeroCelular.ToString()
        };
        AsyncCrearUsuario(a);
    }

    private static async UniTask AsyncCrearUsuario(RegisterUserModel usuarioModelEnviar)
    {
        try
        {
            var json = usuarioModelEnviar.ToJson();
            UnityWebRequest response = new UnityWebRequest(rutaPrincipal + "/usuario/register/demo/" + nombreJuego, "POST");
            SetUnityWebRequest(response, json);
            //Debug.Log("JsonEnviado: " + json);
            await UniTask.Delay(100);
            await response.SendWebRequest();
            if (response.result == UnityWebRequest.Result.Success)
            {
                var jsonString = response.downloadHandler.text;
                Respuesta_Modelo<UsuarioModel> respuestaModelo = JsonUtility.FromJson<Respuesta_Modelo<UsuarioModel>>(jsonString);
                _Estado = respuestaModelo.response.error;
                _Mensaje = respuestaModelo.response.msg;

                if (respuestaModelo.response.info != "")
                {
                    _tokenUsuario = respuestaModelo.response.info;
                    DecodeJWT(_tokenUsuario);
                    _startTime = DateTime.Now;
                    //Debug.Log("jsonString: " + jsonString);
                    //Debug.Log("Fecha y Hora Inicial: " + _startTime.ToString("yyyy-MM-dd HH:mm:ss") + ". Token: " + _tokenUsuario);
                }

                //Respuesta_Modelo<object> respuestaModeloA = JsonUtility.FromJson<Respuesta_Modelo<object>>(jsonString);
                //print("RespuestaModelo: " + jsonString + ". Nombre: " + _Nombre + ". Correo: " + _Correo);
            }
            else
            {
                var jsonString = response.downloadHandler.text;
                //print("Json1: " + jsonString);
                Respuesta_Modelo<ErrorModel> respuestaModelo = JsonUtility.FromJson<Respuesta_Modelo<ErrorModel>>(jsonString);
                _Estado = respuestaModelo.response.error;
                foreach (var item in respuestaModelo.response.message)
                {
                    _Mensaje = item + "\n";
                }
                //print("Estado: " + _Estado + ". Mensaje: " + _Mensaje + ". Json: " + jsonString);
            }
        }
        catch(Exception ex)
        {
            //Debug.LogError("Excepción: " + ex.Message);
            // Extraer el JSON válido desde la excepción
            string jsonString = ex.Message;
            int jsonStartIndex = jsonString.IndexOf('{'); // Encontrar el inicio del JSON
            if (jsonStartIndex >= 0)
            {
                jsonString = jsonString.Substring(jsonStartIndex); // Obtener solo el JSON
            }
            //print("json: " + jsonString);
            // Intentar deserializar el JSON
            ErrorModel respuestaModelo = JsonUtility.FromJson<ErrorModel>(jsonString);

            //_Mensaje = string.Join("\n", respuestaModelo.message);
            _Mensaje = respuestaModelo.message[0];

            //print("Mensaje: " + _Mensaje);
        }
    }
    public static void CrearEditarObjetoInventario()
    {
        AsyncCrearEditarObjetoInventario();
    }
    private static async UniTask AsyncCrearEditarObjetoInventario()
    {
        if (_listaObjetosInventario.Count == 0) return;
        if (_tokenUsuario == "" || HasThirtyMinutesPassed())
            ObtenerUsuario();
        // Agregar la lista recibida como parámetro a una lista de objetos
        List<ObjetoModel> listaObjetos = new List<ObjetoModel>();
        foreach (var item in _listaObjetosInventario)
        {
            listaObjetos.Add(
                new ObjetoModel(
                    (int)item.x,
                    (int)item.y
                ));
        }
        //ObtenerRanking();
        // Serializar la lista de objetos a JSON
        string json = JsonUtility.ToJson(new ListObjectModel { objetos = listaObjetos });

        // Quitar el wrapper innecesario y manejar manualmente el formato correcto
        json = json.Substring(11, json.Length - 12); // Quitar "{ \"objetos\": [" y el "]}" final
        //Debug.Log("JSON enviado: " + json);

        UnityWebRequest response = new UnityWebRequest(rutaPrincipal + "/objeto/create/usuario", "POST");
        SetUnityWebRequest(response, json);
        // Agregar el encabezado de autorización
        string authHeader = "Bearer " + _tokenUsuario;
        response.SetRequestHeader("Authorization", authHeader);
        //print("Token: " + _tokenUsuario);        
        await response.SendWebRequest();
        if (response.result == UnityWebRequest.Result.Success)
        {
            var jsonString = response.downloadHandler.text;
            Respuesta_Modelo<object> respuestaModeloA = JsonUtility.FromJson<Respuesta_Modelo<object>>(jsonString);

            string listaString = "";
            foreach (var item in _listaObjetosInventario)
                listaString = listaString + "\n Usuario: " + item[0] + ". Objeto: " + item[1];
            print("Respuesta Inventarios Count: " + _listaObjetosInventario.Count + ". Lista:" + listaString);
            _listaObjetosInventario.Clear();
            //_Mensaje = respuestaModeloA.response;
            //_Estado = respuestaModeloA.info;
        }
    }
    public static void ObtenerObjetoInventarioPorUsuarioToken()
    {
        AsyncObtenerObjetoInventarioPorUsuarioToken(_tokenUsuario);
    }
    private static async UniTask AsyncObtenerObjetoInventarioPorUsuarioToken(string token)
    {
        if(_tokenUsuario == "" || HasThirtyMinutesPassed())
            ObtenerUsuario();
        UsuarioModel inventarioModelEnviar = new UsuarioModel()
        {
            info = token
        };
        var json = inventarioModelEnviar.ToJson();
        UnityWebRequest response = new UnityWebRequest(rutaPrincipal + "/objeto/list_usuario", "GET");
        SetUnityWebRequest(response, json);
        // Agregar el encabezado de autorización
        string authHeader = "Bearer " + _tokenUsuario;
        response.SetRequestHeader("Authorization", authHeader);

        //print("Token: " + _tokenUsuario);
        await response.SendWebRequest();
        //Debug.Log("Esperando datos de usuario");
        if (response != null)
        {
            var jsonString = response.downloadHandler.text;
            //Debug.Log("JSON Inventario: " + jsonString);
            Respuesta_Modelo<InventarioModel> respuestaModelo = JsonUtility.FromJson<Respuesta_Modelo<InventarioModel>>(jsonString);
            //print("Respuesta Inventario: " + respuestaModelo.response.lista.Count);
            string stringLista = "";
            string objeto;
            for (int i = 0; i < respuestaModelo.response.lista.Count; i++)
            {
                objeto = "Nulo";
                if (respuestaModelo.response.lista[i].identificador == 1) { _bestPoints = respuestaModelo.response.lista[i].cantidad; objeto = "BestPoints"; }
                if (respuestaModelo.response.lista[i].identificador == 2) { _points = respuestaModelo.response.lista[i].cantidad; objeto = "Points"; }
                if (respuestaModelo.response.lista[i].identificador == 3) { _coins = respuestaModelo.response.lista[i].cantidad; objeto = "Coins"; }
                if (respuestaModelo.response.lista[i].identificador == 4) { _nivel = respuestaModelo.response.lista[i].cantidad; objeto = "Nivel"; }
                stringLista = stringLista + "\n{Objeto: " + objeto + ". Identificador: " + respuestaModelo.response.lista[i].identificador + ". Cantidad: " + respuestaModelo.response.lista[i].cantidad + "}";
                //print("identificador: " + respuestaModelo.response.lista[i].identificador + ". Cantidad: " + respuestaModelo.response.lista[i].cantidad);
            }
            Debug.Log("Inventario: " + stringLista);
            //ObtenerRanking();
        }
    }

    public static async UniTask ObtenerRanking()
    {
        Permiso();
        await AsyncObtenerRanking(_tokenUsuario, 25, 1);
    }

    private static async UniTask AsyncObtenerRanking(string token, int cantidad, int id)
    {
        try
        {
            if (_tokenUsuario == "" || HasThirtyMinutesPassed())
                await ObtenerUsuario();

            UsuarioModel modelo = new UsuarioModel()
            {
                info = token
            };
            while (token == "")
            {
                await UniTask.Delay(50); // Espera 50 milisegundos antes de verificar nuevamente
            }
            var json = modelo.ToJson();
            Debug.Log("JSON enviado para obtener el ranking: " + json);
            UnityWebRequest response = new UnityWebRequest(rutaPrincipal + "/objeto/list/" + cantidad + "?identificador=" + id, "GET");
            //response.certificateHandler = new AcceptAllCertificatesSignedWithASpecificKeyPublicKey();

            SetUnityWebRequest(response, json);

            // Agregar el encabezado de autorización
            string authHeader = "Bearer " + token;
            Debug.Log("JSON enviado con el token para el ranking: " + token);

            response.SetRequestHeader("Authorization", authHeader);
            await response.SendWebRequest();
            Debug.Log("Response.DownloadHandler.text Ranking: " + response.downloadHandler.text);
            if (response != null)
            {
                var jsonString = response.downloadHandler.text;
                Debug.Log("JSON Ranking: " + jsonString);
                respuestaRanking = JsonUtility.FromJson<Respuesta_Modelo<RankingModel>>(jsonString);
                //string listaString = "";
                //foreach (var item in respuestaRanking.response.lista)
                //    listaString = listaString + "\n Usuario: " + item.usuario.nombre + ". Objeto: " + item.objeto.cantidad;
                //print("Respuesta Ranking Count: " + respuestaRanking.response.lista.Count + ". Lista:" + listaString);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Excepción: " + ex.Message);
        }
    }
    public static void SetUnityWebRequest(UnityWebRequest response, string json)
    {
        response.certificateHandler = new AcceptAllCertificatesSignedWithASpecificKeyPublicKey();
        response.SetRequestHeader("Content-Type", "application/json");
        response.SetRequestHeader("site", "du2xvM273RbjG31mBdGH3qokOdfi8aK2yFnQy0s1RGYV");//clave
        byte[] rawData = Encoding.UTF8.GetBytes(json);
        response.uploadHandler = new UploadHandlerRaw(rawData);
        response.downloadHandler = new DownloadHandlerBuffer();
    }
    public static void DecodeJWT(string token)
    {
        try
        {
            string[] parts = token.Split('.');
            if (parts.Length != 3)
            {
                //Debug.LogError("Invalid token format");
                return;
            }
            string payload = parts[1];
            string decodedPayload = Base64UrlDecode(payload);
            UserToken _token = JsonUtility.FromJson<UserToken>(decodedPayload);
            _Nombre = _token.nombre;
            _Correo = _token.correo;
        }
        catch (Exception ex)
        {
            //Debug.LogError("An error occurred while decoding the token: " + ex.Message);
        }
    }

    private static string Base64UrlDecode(string input)
    {
        string output = input.Replace('-', '+').Replace('_', '/');
        switch (output.Length % 4)
        {
            case 0: break;
            case 2: output += "=="; break;
            case 3: output += "="; break;
            default: throw new FormatException("Invalid Base64 URL string.");
        }
        byte[] byteArray = Convert.FromBase64String(output);
        return Encoding.UTF8.GetString(byteArray);
    }
    private static void Permiso()
    {
        System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
    }
    class AcceptAllCertificatesSignedWithASpecificKeyPublicKey : CertificateHandler
    {
        private static string PUB_KEY = "mypublickey";
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true;
        }
    }
    // Método que compara la fecha y hora actuales con la fecha y hora inicial
    public static bool HasThirtyMinutesPassed()
    {
        int minutesTime = 30;
        _currentTime = DateTime.Now;
        TimeSpan timeDifference = _currentTime - _startTime;
        bool hasToken = timeDifference.TotalMinutes > minutesTime;
        //Debug.Log("Fecha y Hora Inicial: " + _startTime.ToString("yyyy-MM-dd HH:mm:ss") + ". Fecha y Hora Actual: " + _currentTime.ToString("yyyy-MM-dd HH:mm:ss") + ". Han transcurrido: " + timeDifference.TotalMinutes + " minutos. Han pasado más de 30 minutos? " + hasToken);
        return hasToken;
    }
}
