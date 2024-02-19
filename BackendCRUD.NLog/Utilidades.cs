using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Reflection;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Mail;
using NLog;
using System.Xml;
using System.Web.UI;
using System.Web.Script.Serialization;
using System.Web.Mvc;
using Common.Security.Crypto;
using System.Security.Cryptography;
using System.IO.Compression;
using System.Text.RegularExpressions;

namespace MedicalWebScraping.NLog
{
    public class Utilidades
    {
        public readonly static string APP_NAME = "Trikuu";
        private static readonly Logger _bitacora = LogManager.GetCurrentClassLogger(); // intancia de clase que permite manejar los Logs
        private static readonly GestorLog _gestorLog = new GestorLog(_bitacora);
        public const int MAX_TIME_CODE = 5;  // en minutos
        public const int MAX_HR_TEMP_KEY = 12;  // tiempo maximo por defecto en horas de validez de clave temporal de registro
        public const int DEFAULT_DAYS_SESSION_OPEN = 7;  // numero de días para recordar una sesion abierta (Cookie)
        public const string MONEDA_TRANSACCION = "$";
        public static readonly string SEPARADOR_LOG = "|";
        public static readonly int ANCHO_ALERTAS = 300;  // ancho por defecto en pixeles  de la ventana de alertas (RadNotification)
        public static readonly int ALTO_ALERTAS = 250;   // alto por defecto en pixeles  de la ventana de alertas (RadNotification)
        public static readonly string MENSAJE_ADMIN_SISTEMA = "Por favor comuníquese con el Administrador del Sistema.";
        public static readonly string IMG_NOT_FOUND = "/Content/Images/Imgno.jpg";
        public static readonly string COSTO_GRATIS = "0";
        public static readonly string PERIODO_GRATIS = "15"; // días
        public static readonly string ANNO_COPYRIGHT = DateTime.Now.Year.ToString();
        public static readonly string CONST_NOMBRE_SITIO = "www.Trikuu.com";
        public static readonly string KHIPU_NOTIF_API_VERSION = "1.3";
        public const int ID_MONEDA_UF = 1;
        public const decimal VALOR_UF = 27234.91m;
        public const decimal VALOR_USD = 667;
        public const decimal VALOR_TARIFA_UF_DEMO = 0.0004m;
        public const int VALOR_TARIFA_CLP_DEMO  = 1;
        public const decimal VALOR_TARIFA_USD_DEMO = 0.01m;        

        // titulos de correos
        public static readonly string PRODUCT_NAME = "Trikuu.com";
        public static readonly string SEPARADOR = " | ";
        public static readonly string TITULO_MAIL_CONTACTO = PRODUCT_NAME + SEPARADOR + "Contacto";
        public static readonly string TITULO_MAIL_CONTACTO_COPY = PRODUCT_NAME + SEPARADOR + "Comprobante Contacto";
        public static readonly string RECOVER_PASS_CLIENT = PRODUCT_NAME + SEPARADOR + "Recuperación de contraseña";
        public static readonly string RECOVER_PASS_OPERATOR = PRODUCT_NAME + SEPARADOR + "Recuperación de contraseña operador";
        public static readonly string CHANGEPASS_CLIENT = PRODUCT_NAME + SEPARADOR + "Notificación de cambio de contraseña";
        public static readonly string CHANGEPASS_OPERATOR = PRODUCT_NAME + SEPARADOR + "Notificación de cambio de contraseña operador";
        public static readonly string WEBRESERVATION_CLIENT = PRODUCT_NAME + SEPARADOR + "Confirmación de reserva de tour";
        public static readonly string NEWCAMPAING_REQ_OPERATOR = PRODUCT_NAME + SEPARADOR + "Solicitud nueva campaña";
        public static readonly string NEWCAMPAING_CONFIRM_OPERATOR = PRODUCT_NAME + SEPARADOR + "Confirmación de pago de servicio";
        public static readonly string NEWCAMPAING_EXPIRED_OPERATOR = PRODUCT_NAME + SEPARADOR + "Campaña vencida";
        public static readonly string WEBRESERVATION_EXPIRED_CUSTOMER = PRODUCT_NAME + SEPARADOR + "Reserva vencida!";
        public static readonly string WEBRESERVATION_EXPIRED_ADMIN = PRODUCT_NAME + SEPARADOR + "Resumen Reservas vencidas";
        public static readonly string NEWCAMPAING_CANCELED_OPERATOR = PRODUCT_NAME + SEPARADOR + "Reserva anulada!";
        public static readonly string WEBRESERVATION_CANCELED_CUSTOMER = PRODUCT_NAME + SEPARADOR + "Reserva anulada";
        public static readonly string WEBRESERVATION_NEXT_NOTICE_CUSTOMER = PRODUCT_NAME + SEPARADOR + "Queda poco para que comience tu tour!";
        public static readonly string WEBRESERVATION_NOTIFIED_ADMIN = PRODUCT_NAME + SEPARADOR + "Resumen reservas notificadas";
        public static readonly string BIRTHDAY_CUSTOMER = PRODUCT_NAME + SEPARADOR + "Feliz cumpleaños!";
        public static readonly string SUSCRIPTION = PRODUCT_NAME + SEPARADOR + "Bienvenido a la lista de suscripción";
        public static readonly string ANNIVERSARY_OPERATOR_1_YEAR = PRODUCT_NAME + SEPARADOR + "Gracias por preferirnos en este primer año!";
        public static readonly string ANNIVERSARY_OPERATOR_2_YEAR = PRODUCT_NAME + SEPARADOR + "Gracias por preferirnos en este segundo año!";
        public static readonly string ANNIVERSARY_OPERATOR_3_YEAR = PRODUCT_NAME + SEPARADOR + "Gracias por preferirnos durante 3 años!";
        public static readonly string ANNIVERSARY_OPERATOR_5_YEAR = PRODUCT_NAME + SEPARADOR + "Gracias por preferirnos durante 5 años año!";
        public static readonly string ANNIVERSARY_OPERATOR_10_YEAR = PRODUCT_NAME + SEPARADOR + "Gracias por preferirnos durante 10 años!";
        public static readonly string WEBPAY_OK_CUSTOMER = PRODUCT_NAME + SEPARADOR + "Confirmación de Pago";
        public static readonly string WEBPAY_ERROR_CUSTOMER = PRODUCT_NAME + SEPARADOR + "Error en Pago";
        public static readonly string NEW_USER = PRODUCT_NAME + SEPARADOR + "Bienvenido";
        public static readonly string NEW_USERVAL = PRODUCT_NAME + SEPARADOR + "Validación nuevo usuario";
        public static readonly string NEW_PROVIDERVAL = PRODUCT_NAME + SEPARADOR + "Validación nuevo proveedor";
        public static readonly string NEW_PROVIDER = PRODUCT_NAME + SEPARADOR + "Bienvenido nuevo proveedor";
        public static readonly string NEW_TOURGUIDE = PRODUCT_NAME + SEPARADOR + "Bienvenido nuevo guía turístico";
        public static readonly string NEW_CAMPAIGN = PRODUCT_NAME + SEPARADOR + "Confirmación nueva campaña";
        public static readonly string PROMOKICKOFF = PRODUCT_NAME + SEPARADOR + "Promoción lanzamiento - Aprovecha nuestras ofertas!";
        public static readonly string PROMONAVIDAD = PRODUCT_NAME + SEPARADOR + "Promoción temporada de navidad - Aprovecha nuestras ofertas!";
        public static readonly string PROMOTEMPALTA = PRODUCT_NAME + SEPARADOR + "Promoción temporada alta - Aprovecha nuestras ofertas!";
        public static readonly string PROMOTEMPBAJA = PRODUCT_NAME + SEPARADOR + "Promoción temporada baja - Aprovecha nuestras ofertas!";
        public static readonly string PROMOESPECIAL = PRODUCT_NAME + SEPARADOR + "Promoción - Aprovecha nuestras ofertas!";
        public static readonly string PRE_REGISTER_PROV = PRODUCT_NAME + SEPARADOR + "Pre-ingreso proveedor de servicios";
        public static readonly string PRE_REGISTER_PROV_COPY = PRODUCT_NAME + SEPARADOR + "Comprobante de Pre-ingreso proveedor de servicios";


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateText"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static DateTime ConvertDate(string dateText)
        {
            string[] temp = dateText.Split(new char[] { ',' });
            string diaNombre = temp[0];

            string[] diaNumeroYHora = temp[1].Trim().Split(new char[] { ' ' });

            int dia = Convert.ToInt32(diaNumeroYHora[0]);
            int mes = 0;
            int hora = Convert.ToInt32(diaNumeroYHora[2].Split(new char[] { ':' })[0]);
            int minuto = Convert.ToInt32(diaNumeroYHora[2].Split(new char[] { ':' })[1]);
            //Traduce el mes

            string mesFound = diaNumeroYHora[1].ToLower();

            if (string.IsNullOrEmpty(mesFound))
                throw new Exception("Mes no encontrado en cadena [" + dateText + "]!!");

            if (dia == 0)
                throw new Exception("Día no encontrado en cadena [" + dateText + "]!!");

            if (hora == 0)
                throw new Exception("Hora no encontrado en cadena [" + dateText + "]!!");

            switch (mesFound)
            {
                case "ene":
                case "enero":
                    mes = 1;
                    break;
                case "feb":
                case "febrero":
                    mes = 2;
                    break;
                case "mar":
                case "marzo":
                    mes = 3;
                    break;
                case "abr":
                case "abril":
                    mes = 4;
                    break;
                case "may":
                case "mayo":
                    mes = 5;
                    break;
                case "jun":
                case "junio":
                    mes = 6;
                    break;
                case "jul":
                case "julio":
                    mes = 7;
                    break;
                case "ago":
                case "agosto":
                    mes = 8;
                    break;
                case "sep":
                case "sept":
                case "septiembre":
                    mes = 9;
                    break;
                case "oct":
                case "octubre":
                    mes = 10;
                    break;
                case "nov":
                case "noviembre":
                    mes = 11;
                    break;
                case "dic":
                case "diembre":
                    mes = 12;
                    break;
                default:
                    throw new Exception("Mes [" + mesFound + "] no identificado!");

            }

            DateTime dtOutput = new DateTime(DateTime.Now.Year, mes, dia, hora, minuto, 0);
            return dtOutput;
        }



        /// <summary>
        /// Metodo para enviar un correo.
        /// </summary>
        /// <param name="servidor"></param>
        /// <param name="puerto"></param>
        /// <param name="De"></param>
        /// <param name="Para">Lista de destinatarios a enviar correo. Separar con ;</param>
        /// <param name="Copia">Lista de destinatarios a enviar copia del correo. Separar con ;</param>
        /// <param name="CopiaOculta">Lista de destinatarios a enviar copia oculta del correo. Separar con ;</param>
        /// <param name="Asunto"></param>
        /// <param name="CuerpoMensaje"></param>
        /// <param name="rutaAtachado"></param>
        /// <param name="userNameCredential"></param>
        /// <param name="passCredential"></param>
        /// <param name="CuerpoHTML">Indica con TRUE si el cuerpo del mensaje va en HTML</param>
        /// <returns></returns>
        public static ClsStatusMail EnviarCorreos(string servidor, string puerto, string De, string Para, string Copia, string CopiaOculta, string Asunto, string CuerpoMensaje, string rutaAtachado, string userNameCredential, string passCredential, bool UseSSL, bool CuerpoHTML, string aliasDe = "", string emailOrigen = "", string nameFilePDF = "", Stream atachadoPDFBase64 = null)
        {
            bool status = false;
            MailMessage nuevoCorreo = null;
            ClsStatusMail salida = new ClsStatusMail();
            salida.Estado = status;

            try
            {
                if (string.IsNullOrEmpty(De))
                {
                    _gestorLog.RegistrarLog(Enumeradores.EnumTiposLog.Error, "Error tratando de enviar correo electrónico. El parámetro de entrada [DE] no puede estar vacío o nulo.");
                    salida.Mensaje = "Error tratando de enviar correo electrónico. El parámetro de entrada [DE] no puede estar vacío o nulo.";
                    return salida;
                }

                if (string.IsNullOrEmpty(Para))
                {
                    _gestorLog.RegistrarLog(Enumeradores.EnumTiposLog.Error, "Error tratando de enviar correo electrónico. El parámetro de entrada [PARA] no puede estar vacío o nulo.");
                    salida.Mensaje = "Error tratando de enviar correo electrónico. El parámetro de entrada [PARA] no puede estar vacío o nulo.";
                    return salida;
                }
                if (string.IsNullOrEmpty(Asunto))
                {
                    _gestorLog.RegistrarLog(Enumeradores.EnumTiposLog.Error, "Error tratando de enviar correo electrónico. El parámetro de entrada [ASUNTO] no puede estar vacío o nulo.");
                    salida.Mensaje = "Error tratando de enviar correo electrónico. El parámetro de entrada [ASUNTO] no puede estar vacío o nulo.";
                    return salida;
                }

                if (string.IsNullOrEmpty(CuerpoMensaje))
                {
                    _gestorLog.RegistrarLog(Enumeradores.EnumTiposLog.Error, "Advertencia al enviar correo electrónico. El parámetro de entrada [CUERPO_MENSAJE] está vacío..");
                }

                nuevoCorreo = new MailMessage();

                // SGC 03-04-2018: Incorpora alias de la cuenta de envío en caso de venir.
                if (!string.IsNullOrEmpty(aliasDe))
                    nuevoCorreo.From = new MailAddress(De, aliasDe);
                else
                    nuevoCorreo.From = new MailAddress(De);

                string[] listaPara = Para.Split(';'); // separa por el simbolo ;

                if (listaPara != null)
                {
                    foreach (string elemento in listaPara)
                    {
                        nuevoCorreo.To.Add(elemento);
                    }
                }

                if (!string.IsNullOrEmpty(Copia))
                {
                    string[] listaCC = Copia.Split(';'); // separa por el simbolo ;

                    if (listaCC != null)
                    {
                        foreach (string elemento in listaCC)
                        {
                            nuevoCorreo.CC.Add(elemento);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(CopiaOculta))
                {
                    string[] listaCCo = CopiaOculta.Split(';'); // separa por el simbolo ;

                    if (listaCCo != null)
                    {
                        foreach (string elemento in listaCCo)
                        {
                            nuevoCorreo.Bcc.Add(elemento);
                        }
                    }
                }

                if (De.Equals(Para) && !string.IsNullOrEmpty(emailOrigen))
                    nuevoCorreo.Subject = Asunto + " [" + emailOrigen + "]";
                else
                    nuevoCorreo.Subject = Asunto;

                nuevoCorreo.Body = CuerpoMensaje;
                nuevoCorreo.IsBodyHtml = CuerpoHTML;

                if (rutaAtachado != "")
                {
                    if (string.IsNullOrEmpty(rutaAtachado) == false)
                    {
                        Attachment atachado = new Attachment(rutaAtachado);                        
                        nuevoCorreo.Attachments.Add(atachado);
                    }
                }
                //NCA 26-06-2019: Se agrega validacion para adjuntar archivos Pdf en Base64.
                if(atachadoPDFBase64 != null)
                {
                    Attachment atachado = new Attachment(atachadoPDFBase64, Path.GetFileName(nameFilePDF + ".pdf"), "application/pdf");
                    nuevoCorreo.Attachments.Add(atachado);
                }

                nuevoCorreo.Priority = System.Net.Mail.MailPriority.Normal;
                //NCA 12-12-2018: Cambiar dirección de correo electronico a contacto@Trikuu.com, ya que se esta usando no-reply@xpsoft.cl para pruebas!.                
                SmtpClient emailClient = new SmtpClient(servidor, Convert.ToInt32(puerto));

                // **** Credenciales para enviar el correo. Mientras se debe usar una cuenta de Usuario existente *****
                emailClient.UseDefaultCredentials = false;
                emailClient.Timeout = 10000;  // 1000 = 1 seg
                emailClient.EnableSsl = UseSSL;
                emailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                NetworkCredential credencial = new NetworkCredential();
                credencial.UserName = userNameCredential;
                credencial.Password = passCredential;
                //NCA 12-12-2018: Se comenta credencial.Domain
                //credencial.Domain = "Trikuu.cl";

                emailClient.Credentials = credencial;
                // **** fin credenciales para enviar el correo ********************************************************

                nuevoCorreo.BodyEncoding = UTF8Encoding.UTF8;
                nuevoCorreo.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                emailClient.Send(nuevoCorreo);
                nuevoCorreo.Dispose();

                if (De.Equals(Para) && !string.IsNullOrEmpty(emailOrigen))
                    _gestorLog.RegistrarLog(Enumeradores.EnumTiposLog.Info, "Correo electrónico enviado correctamente De [" + De + "] Para [" + Para + "]. Asunto [" + Asunto + "] Datos de conexion: Server [" + servidor + "]  Port [" + puerto + "] SSL [" + (UseSSL == true ? "SI" : "NO") + "] Cuenta de salida [" + userNameCredential + "]. Email origen [" + emailOrigen + "]");
                else
                    _gestorLog.RegistrarLog(Enumeradores.EnumTiposLog.Info, "Correo electrónico enviado correctamente De [" + De + "] Para [" + Para + "]. Asunto ["+Asunto+"] Datos de conexion: Server [" + servidor + "]  Port [" + puerto + "] SSL [" + (UseSSL == true ? "SI" : "NO") + "] Cuenta de salida [" + userNameCredential + "].");

                salida.Mensaje = "Tu mensaje fue enviado correctamente a " + Para + ". Te contactaremos a la brevedad.";
                status = true;
            }
            catch (SmtpException ex)
            {
                status = false;
                string mensaje = "";
                if (De.Equals(Para) && !string.IsNullOrEmpty(emailOrigen))
                    mensaje = "Error SMTP enviando correo De [" + De + "] Para [" + Para + "]: Datos de conexion: Server [" + servidor + "]  Port [" + puerto + "] SSL [" + (UseSSL == true ? "SI" : "NO") + "] Cuenta de salida [" + userNameCredential + "]. Email origen[" + emailOrigen + "] Det: " + ex.Message;
                else
                    mensaje = "Error SMTP enviando correo De [" + De + "] Para [" + Para + "]: Datos de conexion: Server [" + servidor + "]  Port [" + puerto + "] SSL [" + (UseSSL == true ? "SI" : "NO") + "] Cuenta de salida [" + userNameCredential + "]. Det: " + ex.Message;

                _gestorLog.RegistrarLog(Enumeradores.EnumTiposLog.Error, mensaje, ex);
                salida.Mensaje = "Error 874 enviando correo electrónico. MSG [" + ex.Message + "]";
            }
            catch (Exception ex)
            {
                status = false;
                string mensaje = "";
                if (De.Equals(Para) && !string.IsNullOrEmpty(emailOrigen))
                    mensaje = "Error enviando correo De [" + De + "] Para [" + Para + "]: Datos de conexion: Server [" + servidor + "]  Port [" + puerto + "] SSL [" + (UseSSL == true ? "SI" : "NO") + "] Cuenta de salida [" + userNameCredential + "]. Email origen[" + emailOrigen + "] Det: " + ex.Message;
                else
                    mensaje = "Error enviando correo De [" + De + "] Para [" + Para + "]: Datos de conexion: Server [" + servidor + "]  Port [" + puerto + "] SSL [" + (UseSSL == true ? "SI" : "NO") + "] Cuenta de salida [" + userNameCredential + "]. Det: " + ex.Message;

                _gestorLog.RegistrarLog(Enumeradores.EnumTiposLog.Error, mensaje, ex);
                salida.Mensaje = "Error 875 enviando correo electrónico. MSG [" + ex.Message + "]";                
            }
            //finally
            //{
            //    if (nuevoCorreo != null)
            //        nuevoCorreo.Dispose();
            //}

            salida.Estado = status;
            return salida;
        }

        /// <summary>
        /// Método encargado de exportar a Excel
        /// </summary>
        /// <param name="dt"></param>
        public static void ExportarGrillaExcel(System.Data.DataTable dt)
        {
            System.Web.HttpResponse oResponse = System.Web.HttpContext.Current.Response;

            oResponse.Clear();
            oResponse.AddHeader("Content-Disposition", "attachment; filename=Exportador_" + DateTime.Now.Date + ".xls");
            oResponse.ContentType = "application/vnd.ms-excel";
            oResponse.Charset = "";

            System.IO.StringWriter stringWrite = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htmlWrite = new System.Web.UI.HtmlTextWriter(stringWrite);
            System.Web.UI.WebControls.DataGrid dg = new System.Web.UI.WebControls.DataGrid();

            dg.DataSource = dt;
            dg.DataBind();
            dg.RenderControl(htmlWrite);

            oResponse.Write(stringWrite.ToString());
            oResponse.End();
        }

        /// <summary>
        /// Clase genérica para convertir una Lista Genérica de elementos en
        /// un objeto DataTable
        /// </summary>
        /// <typeparam name="T">Tipo de datos de los elementos de la Lista. 
        /// Debe ser una clase con un constructor sin parámetros.</typeparam>
        public static class Converter<T> where T : new()
        {
            public static DataTable Convertir(List<T> items)
            {
                // Instancia del objeto a devolver
                DataTable returnValue = new DataTable();
                // Información del tipo de datos de los elementos del List
                Type itemsType = typeof(T);
                // Recorremos las propiedades para crear las columnas del datatable
                foreach (PropertyInfo prop in itemsType.GetProperties())
                {
                    // Crearmos y agregamos una columna por cada propiedad de la entidad
                    DataColumn column = new DataColumn(prop.Name);
                    column.DataType = prop.PropertyType;
                    returnValue.Columns.Add(column);
                }

                int j;
                // ahora recorremos la colección para guardar los datos
                // en el DataTable
                foreach (T item in items)
                {
                    j = 0;
                    object[] newRow = new object[returnValue.Columns.Count];
                    // Volvemos a recorrer las propiedades de cada item para
                    // obtener su valor guardarlo en la fila de la tabla
                    foreach (PropertyInfo prop in itemsType.GetProperties())
                    {
                        newRow[j] = prop.GetValue(item, null);
                        j++;
                    }
                    returnValue.Rows.Add(newRow);
                }
                // Devolver el objeto creado
                return returnValue;
            }
        }

        /// <summary>
        /// Método que obtiene el valor de una clave de la sección APP Setting del WEB.CONFIG
        /// </summary>
        /// <param name="clave"></param>
        /// <returns></returns>
        public static string TraerClaveAppSetting(string clave)
        {
            string variable = string.Empty;

            try
            {
                //System.Configuration.Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                //variable = config.AppSettings.Settings[clave].Value;
                variable = System.Configuration.ConfigurationManager.AppSettings[clave];
            }
            catch (Exception ex)
            {
                _gestorLog.RegistrarLog(Enumeradores.EnumTiposLog.Error, "Error leyendo clave [" + clave + "]", ex);
            }

            return variable;
        }


        /// <summary>
        /// Método que obtiene el valor de una clave de la sección APP Setting del WEB.CONFIG
        /// </summary>
        /// <param name="clave"></param>
        /// <param name="sDefault"
        /// <returns></returns>
        public static string TraerClaveAppSetting(string clave, string sDefault)
        {
            string variable = string.Empty;

            try
            {
                //System.Configuration.Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                //variable = config.AppSettings.Settings[clave].Value;
                variable = System.Configuration.ConfigurationManager.AppSettings[clave];
            }
            catch (Exception ex)
            {
                _gestorLog.RegistrarLog(Enumeradores.EnumTiposLog.Error, "Error leyendo clave [" + clave + "]", ex);
                return sDefault;
            }

            return variable;
        }


        public static string TraerImagenWeb(string Imagen)
        {
            try
            {
                if (string.IsNullOrEmpty(Imagen))
                {
                    //Utilidades.TraerClaveAppSetting("PATH_IMG", "~/Content/PublicImg")                   
                    return IMG_NOT_FOUND;
                }
                //else if (!System.IO.File.Exists(Server.MapPath(Imagen)))
                //    return IMG_NOT_FOUND;
                else
                    return Imagen; 

            }
            catch (Exception ex)
            {
                _gestorLog.RegistrarLog(Enumeradores.EnumTiposLog.Error, ex);
                return string.Empty;
            }
        }


        /// <summary>
        /// Lector de variables de sesión
        /// </summary>
        /// <param name="u">Pagina o Control que contiene las sesiones</param>
        /// <param name="variable">Nombre de la variable de Sesion</param>
        /// <returns></returns>
        public static string LectorSession(UserControl u, string variable)
        {
            string cadena = string.Empty;

            try
            {
                if (u == null || variable == null) // si no existe la variable de sesión
                    cadena = string.Empty;
                else
                {
                    if (u.Session[variable] == null) // si no existe la variable de sesión
                        cadena = string.Empty;
                    else
                        cadena = u.Session[variable].ToString();
                }
            }
            catch (Exception ex)
            {
                _gestorLog.RegistrarLog(Enumeradores.EnumTiposLog.Error, ex);
                cadena = string.Empty;
            }

            return cadena;
        }


        /// <summary>
        /// Funcion que descompone un rut en sus partes numerica y dígito verificador
        /// </summary>
        /// <param name="Rut"></param>
        /// <returns></returns>
        public static string[] ExtraeRut(string Rut)
        {
            if (string.IsNullOrEmpty(Rut) == false)
            {
                Rut = Rut.Replace(".", ""); // se le quita los caracteres extraños
                Rut = Rut.Replace(",", "");

                string[] varTemporal;

                if (Rut.Contains("-")) // si contiene el guión
                {
                    varTemporal = Rut.Split(new Char[] { '-' }); // separa por el guión    
                }
                else
                { // si no tiene el guion
                    varTemporal = new string[2];

                    varTemporal[0] = Rut.Substring(1, Rut.Length - 1);
                    varTemporal[1] = Rut.Substring(Rut.Length - 1, 1);
                }
                return varTemporal;
            }
            else
                return null;
        }

        /// <summary>
        /// Función que extrae la parte numérica de un Rut
        /// </summary>
        /// <param name="Rut"></param>
        /// <returns></returns>
        public static int ExtraerParteNumericaRut(string Rut)
        {
            int tmp = -1;

            try
            {
                if (string.IsNullOrEmpty(Rut) == false)
                {
                    if (Rut.Length >= 2)
                    {
                        foreach (string j in ExtraeRut(Rut))
                        {
                            return Convert.ToInt32(j);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return tmp;
            }
            return tmp;
        }

        /// <summary>
        /// Función que extrae la parte del dígito verificador
        /// </summary>
        /// <param name="Rut"></param>
        /// <returns></returns>
        public static string ExtraerParteDigVerificadorRut(string Rut)
        {
            string tmp = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(Rut) == false)
                {
                    foreach (string j in ExtraeRut(Rut))
                    {
                        tmp = j; // el ultimo valor será el dígito verificador
                    }
                }
            }
            catch (Exception ex)
            {
                return tmp;
            }
            return tmp;
        }


        /// <summary>
        /// Método que retorna TRUE si la expresion es NUMERICA
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public static System.Boolean IsNumeric(System.Object Expression)
        {
            if (Expression == null || Expression is DateTime || Expression is Boolean)
                return false;

            if (Expression is Int16 || Expression is Int32 || Expression is Int64 || Expression is Decimal || Expression is Single || Expression is Double)
                return true;

            try
            {
                if (Expression is string)
                    Double.Parse(Expression as string);
                else
                    Double.Parse(Expression.ToString());
                return true;
            }
            catch { } // just dismiss errors but return false
            return false;
        }

        //public static string ReadXmlToXElement(string InputUrl, string ElementName, string Default = null)
        //{
        //    XElement elemento = SimpleStreamAxis(InputUrl, ElementName).SingleOrDefault();
        //    if (elemento != null)
        //        return elemento.Value;
        //    else
        //        return null;
        //}


        //public static IEnumerable<XElement> SimpleStreamAxis(string inputUrl, string elementName)
        //{
        //    using (XmlReader reader = XmlReader.Create(inputUrl))
        //    {
        //        reader.MoveToContent();
        //        while (reader.Read())
        //        {
        //            if (reader.NodeType == XmlNodeType.Element)
        //            {
        //                if (reader.Name == elementName)
        //                {
        //                    XElement el = XNode.ReadFrom(reader) as XElement;
        //                    if (el != null)
        //                    {
        //                        yield return el;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        public static string CalculaFechaProx(DateTime? fecha, Common.Enumeradores.TipoSalida Salida)
        {
            string tmp = string.Empty;

            if (fecha == null)
            {
                if (Salida == Enumeradores.TipoSalida.Texto)
                    tmp = "Sin fecha definida";
                else
                    tmp = "br-white";
            }
            else
            {
                DateTime fechaTmp = (DateTime)fecha;

                if ((fechaTmp - DateTime.Now).TotalMinutes <= 0)
                {
                    if (Salida == Enumeradores.TipoSalida.Texto)
                        tmp = "Llegaste tarde: ya venció.";
                    else
                        tmp = "br-grey";
                }
                else if ((fechaTmp - DateTime.Now).TotalMinutes < 60)
                {
                    if (Salida == Enumeradores.TipoSalida.Texto)
                        tmp = "Inicia en menos de 1 hora. Reserva ya!";
                    else
                        tmp = "br-red";
                }
                else if ((fechaTmp - DateTime.Now).TotalMinutes == 60)
                {
                    if (Salida == Enumeradores.TipoSalida.Texto)
                        tmp = "Inicia en 1 hora. Reserva ya!";
                    else
                        tmp = "br-red";
                }
                else if ((fechaTmp - DateTime.Now).TotalHours > 1 && (fechaTmp - DateTime.Now).TotalHours < 24)
                {
                    if (Salida == Enumeradores.TipoSalida.Texto)
                        tmp = "Inicia en " + Math.Round((fechaTmp - DateTime.Now).TotalHours, 0) + " horas. Apúrate";
                    else
                        tmp = "br-orange";
                }
                else if ((fechaTmp - DateTime.Now).TotalDays == 1)
                {
                    if (Salida == Enumeradores.TipoSalida.Texto)
                        tmp = "Inicia en 1 día";
                    else
                        tmp = "br-orange-soft";
                }
                else if ((fechaTmp - DateTime.Now).TotalDays > 1 && (fechaTmp - DateTime.Now).TotalDays < 7)
                {
                    if (Salida == Enumeradores.TipoSalida.Texto)
                        tmp = "Inicia en " + Math.Round((fechaTmp - DateTime.Now).TotalDays, 0) + " días";
                    else
                        tmp = "br-green";
                }
                else if ((fechaTmp - DateTime.Now).TotalDays == 7)
                {
                    if (Salida == Enumeradores.TipoSalida.Texto)
                        tmp = "Inicia en 1 semana";
                    else
                        tmp = "br-green-soft";
                }
                else if ((fechaTmp - DateTime.Now).TotalDays > 7 && (fechaTmp - DateTime.Now).TotalDays < 28)
                {
                    if (Salida == Enumeradores.TipoSalida.Texto)
                        tmp = "Inicia en " + Math.Round((fechaTmp - DateTime.Now).TotalDays / 4, 0) + " semanas";
                    else
                        tmp = "br-green-soft2";
                }
                else if ((fechaTmp - DateTime.Now).TotalDays == 30 || (fechaTmp - DateTime.Now).TotalDays == 31 || (((fechaTmp - DateTime.Now).TotalDays == 28) && fechaTmp.Month == 2))
                {
                    if (Salida == Enumeradores.TipoSalida.Texto)
                        tmp = "Inicia en 1 mes";
                    else
                        tmp = "br-green-soft2";
                }
                else if ((((fechaTmp - DateTime.Now).TotalDays > 28 && fechaTmp.Month == 2) || ((fechaTmp - DateTime.Now).TotalDays > 31 && fechaTmp.Month != 2)) && (fechaTmp - DateTime.Now).TotalDays < 365)
                {
                    if (fechaTmp.Month == 2)
                    {
                        if (Salida == Enumeradores.TipoSalida.Texto)
                            tmp = "Inicia en " + Math.Round((fechaTmp - DateTime.Now).TotalDays / 28, 0) + " meses";
                        else
                            tmp = "br-green-soft2";
                    }
                    else
                    {
                        if (Salida == Enumeradores.TipoSalida.Texto)
                            tmp = "Inicia en " + Math.Round((fechaTmp - DateTime.Now).TotalDays / 30, 0) + " meses";
                        else
                            tmp = "br-green-soft2";
                    }
                }
                else if ((fechaTmp - DateTime.Now).TotalDays == 365)
                {
                    if (Salida == Enumeradores.TipoSalida.Texto)
                        tmp = "Inicia en1 año";
                    else
                        tmp = "br-green-soft2";
                }
                else if ((fechaTmp - DateTime.Now).TotalDays > 365)
                {
                    if (Salida == Enumeradores.TipoSalida.Texto)
                        tmp = "Inicia en mas de 1 año";
                    else
                        tmp = "br-green-soft2";
                }
            }

            return tmp;
        }

        /// <summary>
        /// SGC 22-11-2017 Método que obtiene un valor de una colección
        /// </summary>
        /// <param name="Input"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetDiccValue(Dictionary<string, string> Input, string key)
        {
            string myValue;

            if (Input == null)
                return string.Empty;
            else
            {
                if (Input.TryGetValue(key, out myValue))
                    return myValue;
            }

            return string.Empty;
        }


        /// <summary>
        /// SGC 24-10-2017 Método que obtiene el valor de un indicador al dia en que se consulta (dolar, dolar_clp, euro, ivp, uf, IPC, utm, imacec, etc)
        /// </summary>
        /// <param name="IndicatorName">dolar, dolar_clp, euro, ivp, uf, IPC, utm, imacec, etc</param>
        /// <param name="Default">Valor por defecto de salida</param>
        /// <returns></returns>
        public string TraerValorIndicadorActual(string IndicatorName, string Default = null)
        {
            try
            {
                string url = Utilidades.TraerClaveAppSetting("IND_URL", Default);

                string jsonString = "{}";
                WebClient http = new WebClient();
                JavaScriptSerializer jss = new JavaScriptSerializer();

                http.Headers.Add(HttpRequestHeader.Accept, "application/json");
                jsonString = http.DownloadString(url);
                var indicatorsObject = jss.Deserialize<Dictionary<string, object>>(jsonString);

                Dictionary<string, Dictionary<string, string>> dailyIndicators = new Dictionary<string, Dictionary<string, string>>();

                int i = 0;
                foreach (var key in indicatorsObject.Keys.ToArray())
                {
                    var item = indicatorsObject[key];

                    if (item.GetType().FullName.Contains("System.Collections.Generic.Dictionary"))
                    {
                        Dictionary<string, object> itemObject = (Dictionary<string, object>)item;
                        Dictionary<string, string> indicatorProp = new Dictionary<string, string>();

                        int j = 0;
                        foreach (var key2 in itemObject.Keys.ToArray())
                        {
                            indicatorProp.Add(key2, itemObject[key2].ToString());
                            j++;
                        }

                        dailyIndicators.Add(key, indicatorProp);
                    }
                    i++;
                }

                return dailyIndicators[IndicatorName]["valor"];

            }
            catch (Exception ex)
            {
                _gestorLog.RegistrarLog(Enumeradores.EnumTiposLog.Error, "Error obteniendo indicador [" + IndicatorName + "]", ex);
            }

            return Default;
        }


        /// <summary>
        /// SGC 24-10-2017 Método que obtiene el valor de un indicador al dia en que se consulta (dolar, dolar_clp, euro, ivp, uf, IPC, utm, imacec, etc)
        /// </summary>
        /// <param name="UrlWS">URL del Servicio (Opcional)</param>
        /// <param name="Default">Valor por defecto de salida</param>
        /// <returns></returns>
        public Dictionary<string, Dictionary<string, string>> TraerIndicadores(string UrlWS = null, string Default = null)
        {
            string url = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(UrlWS))
                    url = Utilidades.TraerClaveAppSetting("IND_URL", Default);
                else
                    url = UrlWS;

                string jsonString = "{}";
                WebClient http = new WebClient();
                JavaScriptSerializer jss = new JavaScriptSerializer();

                http.Headers.Add(HttpRequestHeader.Accept, "application/json");
                jsonString = http.DownloadString(url);
                var indicatorsObject = jss.Deserialize<Dictionary<string, object>>(jsonString);

                Dictionary<string, Dictionary<string, string>> dailyIndicators = new Dictionary<string, Dictionary<string, string>>();

                int i = 0;
                foreach (var key in indicatorsObject.Keys.ToArray())
                {
                    var item = indicatorsObject[key];

                    if (item.GetType().FullName.Contains("System.Collections.Generic.Dictionary"))
                    {
                        Dictionary<string, object> itemObject = (Dictionary<string, object>)item;
                        Dictionary<string, string> indicatorProp = new Dictionary<string, string>();

                        int j = 0;
                        foreach (var key2 in itemObject.Keys.ToArray())
                        {
                            indicatorProp.Add(key2, itemObject[key2].ToString());
                            j++;
                        }

                        dailyIndicators.Add(key, indicatorProp);
                    }
                    i++;
                }

                return dailyIndicators;

            }
            catch (Exception ex)
            {
                _gestorLog.RegistrarLog(Enumeradores.EnumTiposLog.Error, "Error obteniendo indicadores [" + url + "].. llamando a método alternativo [TraerIndicadores]...", ex);
            }

            return null;
        }


        public Dictionary<string, Dictionary<string, string>> TraerValorIndicadorActualv2(string IndicatorName, string Default = null)
        {
            string url = string.Empty;
            string fecha = string.Empty;

            try
            {
                Dictionary<string, Dictionary<string, string>> dailyIndicators = new Dictionary<string, Dictionary<string, string>>();

                url = Utilidades.TraerClaveAppSetting("IND_URL_V2", Default);

                XmlDocument xml = new XmlDocument();
                xml.Load(url);

                XmlElement root = xml.DocumentElement;

                // Check to see if the element has a genre attribute.
                if (root.HasAttribute("date"))
                {
                    fecha = root.GetAttribute("date");
                }

                XmlNode Date = xml.GetElementsByTagName("date").Item(0);
                XmlNode Dolar = xml.GetElementsByTagName("dolar").Item(0);
                XmlNode Uf = xml.GetElementsByTagName("uf").Item(0);

                Dictionary<string, string> lista = new Dictionary<string, string>();

                lista.Add("fecha", fecha);
                lista.Add("valor", Dolar.InnerText.Replace("$", ""));

                dailyIndicators.Add("dolar", lista);

                lista = new Dictionary<string, string>();

                lista.Add("fecha", fecha);
                lista.Add("valor", Uf.InnerText.Replace("$", ""));

                dailyIndicators.Add("uf", lista);

                return dailyIndicators;

            }
            catch (Exception ex)
            {
                _gestorLog.RegistrarLog(Enumeradores.EnumTiposLog.Error, "Error obteniendo indicadores [" + url + "]", ex);
            }

            return null;
        }

        /// <summary>
        /// SGC 02-03-2018: Método que enmascara un email
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static string MaskMail(string Input)
        {
            if (string.IsNullOrEmpty(Input))
                return string.Empty;
            else
            {

                int posArr = Input.IndexOf("@");
                if (posArr != -1)
                {
                    string inicio = Input.Substring(0, posArr);
                    string fin = Input.Substring(posArr - 1, 1);
                    string fin2 = Input.Substring(posArr);
                    string final = string.Empty;

                    for (int k = 1; k < inicio.Length - 1; k++)
                    {
                        final = final + "X";
                    }

                    string final2 = inicio.Substring(0, 1) + final + fin + fin2;
                    return final2;
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Método que devuelve true si es impar
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool EsImpar(int value)
        {
            return value % 2 != 0;
        }

        public static string GenerateURLToken(Dictionary<string, string> parametros)
        {
            int cont = 0;
            string url = String.Empty;
            string urlEnc = String.Empty;
            string salida = String.Empty;
            foreach (KeyValuePair<string, string> par in parametros)
            {
                if (cont > 0)
                {
                    url += "&";
                }

                url += par.Key + "=" + par.Value;
                cont++;
            }
            urlEnc = Utilidades.EncodeURL(Util.Encriptar(url));
            salida = "token=" + urlEnc;
            return salida;
        }

        public static string GenerateURLTokenExpiration(Dictionary<string, string> parametros, int horas)
        {
            DateTime dt = DateTime.Now;
            dt = dt.AddHours(horas);
            string fechaHoraExp = dt.ToString("yyyyMMddHHmmss");
            parametros.Add("expiracion", fechaHoraExp);

            return GenerateURLToken(parametros);
        }

        public static bool ValidaToken(string token)
        {
            string fechaHora = String.Empty;
            string tokenDes = Util.Desencriptar(Utilidades.DecodeURL(token));

            String[] valores = tokenDes.Split('&');

            foreach (var val in valores)
            {
                String[] item = val.Split('=');
                if (item[0] == "expiracion")
                {
                    fechaHora = item[1];
                    break;
                }
            }

            if (fechaHora == String.Empty)
            {
                return false;
            }

            DateTime fechaHoraExp = DateTime.ParseExact(fechaHora, "yyyyMMddHHmmss",
                                       System.Globalization.CultureInfo.InvariantCulture);

            DateTime fechaHoraActual = DateTime.Now;

            if (fechaHoraExp < fechaHoraActual)
            {
                return false; // token expirado
            }
            else
            {
                return true;
            }
        }

        public static Dictionary<string, string> GetValuesFromToken(string token)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            string tokenDes = Util.Desencriptar(Utilidades.DecodeURL(token));

            String[] valores = tokenDes.Split('&');

            foreach (var val in valores)
            {
                string k = String.Empty;
                string v = String.Empty;
                String[] item = val.Split('=');
                if (item[0] == null)
                {
                    k = "";
                }
                else
                {
                    k = item[0];
                }
                if (item[1] == null)
                {
                    v = "";
                }
                else
                {
                    v = item[1];
                }
                dic.Add(k, v);
            }

            return dic;
        }

        public static string EncodeURL(string Input)
        {
            char[] padding = { '=' };
            var inputBytes = System.Text.Encoding.UTF8.GetBytes(Input);
            string returnValue = System.Convert.ToBase64String(inputBytes).TrimEnd(padding).Replace('+', '-').Replace('/', '_');

            return returnValue;
        }

        public static string DecodeURL(string Input)
        {
            string incoming = Input.Replace('_', '/').Replace('-', '+');

            switch (Input.Length % 4)
            {
                case 2: incoming += "=="; break;
                case 3: incoming += "="; break;
            }
            byte[] bytes = Convert.FromBase64String(incoming);
            string originalText = Encoding.ASCII.GetString(bytes);

            return originalText;
        }

        /// <summary>
        /// 02-04-2018: Obtiene el título por defecto para enviar mails
        /// </summary>
        /// <param name="TipoPlantilla"></param>
        /// <returns></returns>
        public static string ObtieneTituloMail(Common.Enumeradores.EnumTipoPlantilla TipoPlantilla)
        {
            // SGC 19-marzo-2018: define el SUBJECT del correo                
            string titulo = string.Empty;

            switch (TipoPlantilla)
            {
                case Common.Enumeradores.EnumTipoPlantilla.Recover_Pass_Customer:
                    titulo = Utilidades.RECOVER_PASS_CLIENT;
                    break;
                case Common.Enumeradores.EnumTipoPlantilla.Recover_Pass_Provider:
                    titulo = Utilidades.RECOVER_PASS_OPERATOR;
                    break;
                case Common.Enumeradores.EnumTipoPlantilla.ChangePass_Client:
                    titulo = Utilidades.CHANGEPASS_CLIENT;
                    break;
                case Common.Enumeradores.EnumTipoPlantilla.ChangePass_Provider:
                    titulo = Utilidades.CHANGEPASS_OPERATOR;
                    break;
                case Common.Enumeradores.EnumTipoPlantilla.WebReservation_Customer:
                    titulo = Utilidades.WEBRESERVATION_CLIENT;
                    break;
                case Common.Enumeradores.EnumTipoPlantilla.NewCampaing_Req_Provider:
                    titulo = Utilidades.NEWCAMPAING_REQ_OPERATOR;
                    break;
                case Common.Enumeradores.EnumTipoPlantilla.NewCampaing_Confirm_Provider:
                    titulo = Utilidades.NEWCAMPAING_CONFIRM_OPERATOR;
                    break;
                case Common.Enumeradores.EnumTipoPlantilla.NewCampaing_Expired_Provider:
                    titulo = Utilidades.NEWCAMPAING_EXPIRED_OPERATOR;
                    break;
                case Common.Enumeradores.EnumTipoPlantilla.WebReservation_Expired_Customer:
                    titulo = Utilidades.WEBRESERVATION_EXPIRED_CUSTOMER;
                    break;
                case Common.Enumeradores.EnumTipoPlantilla.WebReservation_Expired_Admin:
                    titulo = Utilidades.WEBRESERVATION_EXPIRED_ADMIN;
                    break;
                case Common.Enumeradores.EnumTipoPlantilla.NewCampaing_Canceled_Provider:
                    titulo = Utilidades.NEWCAMPAING_CANCELED_OPERATOR;
                    break;
                case Common.Enumeradores.EnumTipoPlantilla.WebReservation_Canceled_Customer:
                    titulo = Utilidades.WEBRESERVATION_CANCELED_CUSTOMER;
                    break;
                case Common.Enumeradores.EnumTipoPlantilla.WebReservation_Next_Notice_Customer:
                    titulo = Utilidades.WEBRESERVATION_NEXT_NOTICE_CUSTOMER;
                    break;
                case Common.Enumeradores.EnumTipoPlantilla.WebReservation_Notified_Admin:
                    titulo = Utilidades.WEBRESERVATION_NOTIFIED_ADMIN;
                    break;
                case Common.Enumeradores.EnumTipoPlantilla.Birthday_Customer:
                    titulo = Utilidades.BIRTHDAY_CUSTOMER;
                    break;
                case Common.Enumeradores.EnumTipoPlantilla.Suscription:
                    titulo = Utilidades.SUSCRIPTION;
                    break;
                case Common.Enumeradores.EnumTipoPlantilla.Anniversary_Operator_1_year:
                    titulo = Utilidades.ANNIVERSARY_OPERATOR_1_YEAR;
                    break;
                case Common.Enumeradores.EnumTipoPlantilla.Anniversary_Operator_2_year:
                    titulo = Utilidades.ANNIVERSARY_OPERATOR_2_YEAR;
                    break;
                case Common.Enumeradores.EnumTipoPlantilla.Anniversary_Operator_3_year:
                    titulo = Utilidades.ANNIVERSARY_OPERATOR_3_YEAR;
                    break;
                case Common.Enumeradores.EnumTipoPlantilla.Anniversary_Operator_5_year:
                    titulo = Utilidades.ANNIVERSARY_OPERATOR_5_YEAR;
                    break;
                case Common.Enumeradores.EnumTipoPlantilla.Anniversary_Operator_10_year:
                    titulo = Utilidades.ANNIVERSARY_OPERATOR_10_YEAR;
                    break;
                case Common.Enumeradores.EnumTipoPlantilla.WebPay_OK_Customer:
                    titulo = Utilidades.WEBPAY_OK_CUSTOMER;
                    break;
                case Common.Enumeradores.EnumTipoPlantilla.WebPay_Error_Customer:
                    titulo = Utilidades.WEBPAY_ERROR_CUSTOMER;
                    break;
                case Common.Enumeradores.EnumTipoPlantilla.New_Customer:
                    titulo = Utilidades.NEW_USER;
                    break;
                case Common.Enumeradores.EnumTipoPlantilla.New_UserVal:
                    titulo = Utilidades.NEW_USERVAL;
                    break;
                case Common.Enumeradores.EnumTipoPlantilla.New_ProviderEval:
                    titulo = Utilidades.NEW_PROVIDERVAL;
                    break;
                case Common.Enumeradores.EnumTipoPlantilla.New_Provider:
                    titulo = Utilidades.NEW_PROVIDER;
                    break;
                case Common.Enumeradores.EnumTipoPlantilla.New_TourGuide:
                    titulo = Utilidades.NEW_TOURGUIDE;
                    break;
                case Common.Enumeradores.EnumTipoPlantilla.New_Campaign:
                    titulo = Utilidades.NEW_CAMPAIGN;
                    break;
                case Common.Enumeradores.EnumTipoPlantilla.Contact_Customer:
                    titulo = Utilidades.TITULO_MAIL_CONTACTO;
                    break;
                case Common.Enumeradores.EnumTipoPlantilla.PromoKickOff:
                    titulo = Utilidades.PROMOKICKOFF;
                    break;
                case Common.Enumeradores.EnumTipoPlantilla.PromoNavidad:
                    titulo = Utilidades.PROMONAVIDAD;
                    break;
                case Common.Enumeradores.EnumTipoPlantilla.PromoTempAlta:
                    titulo = Utilidades.PROMOTEMPALTA;
                    break;
                case Common.Enumeradores.EnumTipoPlantilla.PromoTempBaja:
                    titulo = Utilidades.PROMOTEMPBAJA;
                    break;
                case Common.Enumeradores.EnumTipoPlantilla.PromoEspecial:
                    titulo = Utilidades.PROMOESPECIAL;
                    break;
                case Common.Enumeradores.EnumTipoPlantilla.PreRegisterProv:
                    titulo = Utilidades.PRE_REGISTER_PROV;
                    break;
                case Common.Enumeradores.EnumTipoPlantilla.Contact_CustomerCopy:
                    titulo = Utilidades.TITULO_MAIL_CONTACTO_COPY;
                    break;
                case Common.Enumeradores.EnumTipoPlantilla.PreRegisterProvCopy:
                    titulo = Utilidades.PRE_REGISTER_PROV_COPY;
                    break;
                default:
                    _gestorLog.RegistrarLog(Common.Enumeradores.EnumTiposLog.Error, "Error: Tipo de plantilla desconocido!");
                    return string.Empty;
            }

            return titulo;
        }

        public static string RenderViewToString(Controller controller, string viewName, object model)
        {
            var context = controller.ControllerContext;

            if (string.IsNullOrEmpty(viewName))
                viewName = context.RouteData.GetRequiredString("action");

            ViewDataDictionary viewData = new ViewDataDictionary(model);

            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(context, viewName);
                var viewContext = new ViewContext(context, viewResult.View, viewData, new TempDataDictionary(), sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }


        /// <summary>
        /// SGC 31-05-2018 - Método que genera firma para el medio de Pago PayU (Entrada: “ApiKey~merchantId~referenceCode~amount~currency”)
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="merchantId"></param>
        /// <param name="referenceCode"></param>
        /// <param name="amount"></param>
        /// <param name="currency"></param>
        /// <param name="TipoAlgoritmo"></param>
        /// <returns></returns>
        public static string GetSignaturePayU(string apiKey, string merchantId, string referenceCode, string amount, string currency, Enumeradores.EnumTipoAlgoritmo TipoAlgoritmo)
        {
            string salida = string.Empty;

            try
            {
                string entrada = apiKey + "~" + merchantId + "~" + referenceCode + "~" + amount + "~" + currency;

                switch (TipoAlgoritmo)
                {
                    case Enumeradores.EnumTipoAlgoritmo.MD5:
                        salida = GetMD5HashData(entrada);
                        break;
                    case Enumeradores.EnumTipoAlgoritmo.SHA1:
                        salida = GetSHA1HashData(entrada);
                        break;
                    case Enumeradores.EnumTipoAlgoritmo.SHA256:
                        salida = GetSHA256HashData(entrada);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                _gestorLog.RegistrarLog(Enumeradores.EnumTiposLog.Error, "Error generando firma PayU. apiKey [" + apiKey + "] merchantId [" + merchantId + "] referenceCode [" + referenceCode + "] amount [" + amount + "] currency [" + currency + "]", ex);
                salida = "-1";
            }

            return salida.ToLower();
        }

        /// <summary>
        /// take any string and encrypt it using MD5 then
        /// return the encrypted data 
        /// </summary>
        /// <param name="data">input text you will enterd to encrypt it</param>
        /// <returns>return the encrypted text as hexadecimal string</returns>
        private static string GetMD5HashData(string data)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(data);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }

        }

        // <summary>
        /// take any string and encrypt it using SHA1 then
        /// return the encrypted data
        /// </summary>
        /// <param name="data">input text you will enterd to encrypt it</param>
        /// <returns>return the encrypted text as hexadecimal string</returns>
        private static string GetSHA1HashData(string data)
        {
            //create new instance of md5
            SHA1 sha1 = SHA1.Create();

            //convert the input text to array of bytes
            byte[] hashData = sha1.ComputeHash(Encoding.Default.GetBytes(data));

            //create new instance of StringBuilder to save hashed data
            StringBuilder returnValue = new StringBuilder();

            //loop for each byte and add it to StringBuilder
            for (int i = 0; i < hashData.Length; i++)
            {
                returnValue.Append(hashData[i].ToString());
            }

            // return hexadecimal string
            return returnValue.ToString();
        }

        public static string GetSHA256HashData(string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            SHA256Managed hashstring = new SHA256Managed();
            byte[] hash = hashstring.ComputeHash(bytes);
            string hashString = string.Empty;
            foreach (byte x in hash)
            {
                hashString += String.Format("{0:x2}", x);
            }
            return hashString;
        }


        /// <summary>
        /// encrypt input text using MD5 and compare it with
        /// the stored encrypted text
        /// </summary>
        /// <param name="inputData">input text you will enterd to encrypt it</param>
        /// <param name="storedHashData">the encrypted text
        ///         stored on file or database ... etc</param>
        /// <returns>true or false depending on input validation</returns>
        private static bool ValidateMD5HashData(string inputData, string storedHashData)
        {
            //hash input text and save it string variable
            string getHashInputData = GetMD5HashData(inputData);

            if (string.Compare(getHashInputData, storedHashData) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// encrypt input text using SHA1 and compare it with
        /// the stored encrypted text
        /// </summary>
        /// <param name="inputData">input text you will enterd to encrypt it</param>
        /// <param name="storedHashData">the encrypted
        ///           text stored on file or database ... etc</param>
        /// <returns>true or false depending on input validation</returns>

        private static bool ValidateSHA1HashData(string inputData, string storedHashData)
        {
            //hash input text and save it string variable
            string getHashInputData = GetSHA1HashData(inputData);

            if (string.Compare(getHashInputData, storedHashData) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// SGC 08-06-2018 - Metodo que obtiene los tipos de pago (1 cuotas, N-Coutas, sin cuotas, etc) para WebPayPlus
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetWebPayPayTypesDicc()
        {
            /** Crea Dictionary con descripción */
            Dictionary<string, string> description = new Dictionary<string, string>();

            description.Add("VD", "Venta Deb&iacute;to");
            description.Add("VN", "Venta Normal");
            description.Add("VC", "Venta en cuotas");
            description.Add("SI", "cuotas sin inter&eacute;s");
            description.Add("S2", "2 cuotas sin inter&eacute;s");
            description.Add("NC", "N cuotas sin inter&eacute;s");

            return description;
        }

        /// <summary>
        /// SGC 08-06-2018 - Metodo que obtiene los codigos de resultado WebPayPlus
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetWebPayPayCodesDicc()
        {
            /** Crea Dictionary con codigos de resultado */
            Dictionary<string, string> codes = new Dictionary<string, string>();

            codes.Add("0", "Transacci&oacute;n aprobada");
            codes.Add("-1", "Rechazo de transacci&oacute;n");
            codes.Add("-2", "Transacci&oacute;n debe reintentarse");
            codes.Add("-3", "Error en transacci&oacute;n");
            codes.Add("-4", "Rechazo de transacci&oacute;n");
            codes.Add("-5", "Rechazo por error de tasa");
            codes.Add("-6", "Excede cupo m&aacute;ximo mensual");
            codes.Add("-7", "Excede l&iacute;mite diario por transacci&oacute;n");
            codes.Add("-8", "Rubro no autorizado");

            return codes;
        }




        /// <summary>
        /// SGC 26-12-2018 - Método que devuelve TRUE si es un Rut válido
        /// </summary>
        /// <param name="Digito"></param>
        /// <returns></returns>
        public static bool EsParteNumericaValida(int RutN)
        {
            if (RutN >= 1 && RutN < 99999999)
                return true;
            else
                return false;
        }

        /// <summary>
        /// SGC 26-12-2018 - Método que devuelve TRUE si es un dígito verificador válido
        /// </summary>
        /// <param name="Digito"></param>
        /// <returns></returns>
        public static bool EsDigitoVerificadorValido(string Digito)
        {
          switch(Digito)
            {
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                case "K":
                    return true;
                default:
                    return false;
            }            
        }


        /// <summary>
        /// SGC 27-12-2018: Función que devuelve TRUE si el mail es válido
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address.Trim() == email.Trim();
            }
            catch
            {
                return false;
            }

        }


        /// <summary>
        /// SGC 27-12-2018 - Método que devuelve TRUE si el Rut es válido. En caso contrario devuelve False.
        /// </summary>
        /// <param name="RutN"></param>
        /// <returns></returns>
        public static bool EsRutValido(string RutCompleto)
        {
            try
            {
                string digitoCalculado = CalcularDigitoVerificador(ExtraerParteNumericaRut(RutCompleto));
                string digitoEntrada = ExtraerParteDigVerificadorRut(RutCompleto);

                if (digitoCalculado == digitoEntrada)
                    return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }

        /// <summary>
        /// SGC 27-12-2018 - Método que devuelve TRUE si el Rut es válido. En caso contrario devuelve False.
        /// </summary>
        /// <param name="RutN"></param>
        /// <param name="RutDv"></param>
        /// <returns></returns>
        public static bool EsRutValido(int RutN, string RutDv)
        {
            try
            {
                string digitoCalculado = CalcularDigitoVerificador(RutN);

                if (digitoCalculado == RutDv)
                    return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }

        /// <summary>
        /// Método que calcula el digito verificador de un Rut, conociiendo la parte numérica de este
        /// </summary>
        /// <param name="RutN"></param>
        /// <returns></returns>
        public static string CalcularDigitoVerificador(int RutN)
        {
            int Digito;
            int Contador;
            int Multiplo;
            int Acumulador;
            string RutDigito;

            Contador = 2;
            Acumulador = 0;

            while (RutN != 0)
            {
                Multiplo = (RutN % 10) * Contador;
                Acumulador = Acumulador + Multiplo;
                RutN = RutN / 10;
                Contador = Contador + 1;
                if (Contador == 8)
                {
                    Contador = 2;
                }
            }

            Digito = 11 - (Acumulador % 11);
            RutDigito = Digito.ToString().Trim();
            if (Digito == 10)
            {
                RutDigito = "K";
            }
            if (Digito == 11)
            {
                RutDigito = "0";
            }
            return (RutDigito);
        }

        #region CCC 15-06-2019 region para comprimir y transaformar imagenes a binario 

        /// <summary>
        /// CCC metodo que retorna un MemoryStream para luego ser transformado a un archivo definido.
        /// </summary>
        /// <param name="_Path"></param>
        /// <returns></returns>
        public static MemoryStream CreaArchivoDeBin(byte[] datos)
        {
            byte[] datosDes = DecompressGZIP(datos);
            MemoryStream ms = new MemoryStream(datosDes);
            return ms;            
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="_Path"></param>
        /// <returns></returns>
        public static byte[] CreaBinDeArchivo(string _Path)
        {
            try
            {
                if (!string.IsNullOrEmpty(_Path))
                {
                    MemoryStream ms = new MemoryStream();
                    using (FileStream file = new FileStream(_Path, FileMode.Open, FileAccess.Read))
                    {
                        file.CopyTo(ms);
                    }
                    byte[] arFile = CompressGZIP(ms.ToArray());
                    return arFile;
                }
            }
            catch (Exception ex)
            {
                _gestorLog.RegistrarLog(Enumeradores.EnumTiposLog.Error, "Error CreaBinDeArchivo: " + ex.Message, ex);                
            }
            
            return null;
        }

        /// <summary>
        /// Método que comprime (algoritmo GZIP) un arreglo de bytes
        /// </summary>
        /// <param name="bytesFileOriginal"></param>
        /// <returns></returns>
        public static byte[] CompressGZIP(byte[] bytesFileOriginal)
        {
            byte[] compressed;

            using (var outStream = new MemoryStream())
            {
                using (GZipStream zipStream = new GZipStream(outStream, CompressionMode.Compress, false))
                {
                    using (MemoryStream ms = new MemoryStream(bytesFileOriginal))
                    {
                        ms.CopyTo(zipStream);
                    }
                }
                compressed = outStream.ToArray();
            }

            return compressed;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gzip"></param>
        /// <returns></returns>
        public static byte[] DecompressGZIP(byte[] gzip)
        {
            using (GZipStream stream = new GZipStream(new MemoryStream(gzip), CompressionMode.Decompress))
            {
                const int size = 4096;
                byte[] buffer = new byte[size];

                using (MemoryStream memory = new MemoryStream())
                {
                    int count = 0;
                    do
                    {
                        count = stream.Read(buffer, 0, size);
                        if (count > 0)
                        {
                            memory.Write(buffer, 0, count);
                        }
                    }
                    while (count > 0);
                    return memory.ToArray();
                }
            }
        }

        #endregion  

        /// <summary>
        /// Encode en Base64 ideal para campos por URL así no van con %20
        /// </summary>
        /// <param name="str">String a codificar</param>
        /// <returns></returns>
        public static string Base64_Encode(string str)
        {
            byte[] encbuff = System.Text.Encoding.UTF8.GetBytes(str);
            return Convert.ToBase64String(encbuff);
        }

        /// <summary>
        /// Decode un string en Base64
        /// </summary>
        /// <param name="str">String a decodificar</param>
        /// <returns></returns>
        public static string Base64_Decode(string str)
        {
            try
            {
                byte[] decbuff = Convert.FromBase64String(str);
                return System.Text.Encoding.UTF8.GetString(decbuff);
            }
            catch
            {
                //si se envia una cadena si codificación base64, mandamos vacio
                return "";
            }
        }

        /// <summary>
        /// SGC 20-05-2019  Método que genera un número RANDOM a partir de un largo (parémtrico)
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string RandomString(int length)
        {
            Random random = new Random();

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        //Sample escaping of invalid windows characters
        public static string EscapeInvalidCharacter(string fileName)
        {
            //Check if there is no invalid characters return the current name
            if (fileName.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) == -1)
            {
                _gestorLog.RegistrarLog(Common.Enumeradores.EnumTiposLog.Info, "Palabra [" + fileName + "] no tiene caracteres inválidos...");
                return fileName;
            }
            else
            {
                //Escape invalid characters
                fileName = Regex.Replace(fileName, @"[^\w\.@-]", "", RegexOptions.None, TimeSpan.FromSeconds(1.5));
                //If all characters are invalid return underscore as a file name
                if (Path.GetFileNameWithoutExtension(fileName).Length == 0)
                {
                    return fileName.Insert(0, "_");
                }

                _gestorLog.RegistrarLog(Common.Enumeradores.EnumTiposLog.Info, "Palabra [" + fileName + "] filtrada correctamente...");

                //Else return the escaped name
                return fileName;
            }
        }

    }

    public class ClsStatusMail
    {
        public bool Estado { get; set; }
        public string Mensaje { get; set; }
    }
}

