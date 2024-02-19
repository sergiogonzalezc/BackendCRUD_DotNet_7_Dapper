using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using NLog;

namespace MedicalWebScraping.NLog
{
    /// <summary>
    /// Clase que maneja el Log
    /// </summary>
    public class GestorLog
    {
        private string _IdComunidad = string.Empty;
        private string _CodigoComunidad = string.Empty;
        private string _NombreServicio = string.Empty;
        private string _Mensaje = string.Empty;
        private string _StackTrace = string.Empty;        
        private Logger _LogInterno = null;

        # region "Contructores"
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="IdComunidad"></param>
        /// <param name="CodigoComunidad"></param>
        /// <param name="NombreServicio"></param>
        /// <param name="Mensaje"></param>
        /// <param name="StackTrace"></param>
        /// <param name="NumDte"></param>
        /// <param name="LogInterno"></param>
        public GestorLog(string IdComunidad, string CodigoComunidad,  string NombreServicio, string Mensaje, string StackTrace, Logger LogInterno)
        {
            _IdComunidad = IdComunidad;
            _CodigoComunidad = CodigoComunidad;
            _NombreServicio = NombreServicio;
            _Mensaje = Mensaje;
            _StackTrace = StackTrace;            
            _LogInterno = LogManager.GetCurrentClassLogger();
        }

        /// <summary>
        /// Constructor por defecto de la clase
        /// </summary>
        public GestorLog()
        {
            _IdComunidad = string.Empty;
            _CodigoComunidad = string.Empty;            
            _NombreServicio = string.Empty;
            _Mensaje = string.Empty;
            _StackTrace = string.Empty;
            
            _LogInterno = LogManager.GetCurrentClassLogger();
        }

        public GestorLog(Logger LogInterno)
        {
            _IdComunidad = string.Empty;
            _CodigoComunidad = string.Empty;            
            _NombreServicio = string.Empty;
            _Mensaje = string.Empty;
            _StackTrace = string.Empty;
            
            _LogInterno = LogInterno;
        }

        #endregion

        # region "Propiedades"

        public string IdComunidad
        {
            get { return this._IdComunidad; }
            set { this._IdComunidad = value; }
        }

        public string CodigoComunidad
        {
            get { return this._CodigoComunidad; }
            set { this._CodigoComunidad = value; }
        }

        public string NombreServicio
        {
            get { return this._NombreServicio; }
            set { this._NombreServicio = value; }
        }

        public string Mensaje
        {
            get { return this.Mensaje; }
            set { this._Mensaje = value; }
        }

        public string StackTrace
        {
            get { return this._StackTrace; }
            set { this._StackTrace = value; }
        }

        public Logger LogInterno
        {
            get { return this._LogInterno; }
            set { this._LogInterno = value; }
        }

        #endregion

        /// <summary>
        /// Método que registra un mensaje y tipo de log
        /// </summary>
        /// <param name="TipoLog"></param>
        /// <param name="Mensaje"></param>
        public void RegistrarLog(Enumeradores.EnumTiposLog TipoLog, string Mensaje)
        {
            string CodCompletoComunidad = string.Empty;

            if (!string.IsNullOrEmpty(IdComunidad) && !string.IsNullOrEmpty(CodigoComunidad))
                CodCompletoComunidad = IdComunidad + "-" + CodigoComunidad;

            string cadena = CodCompletoComunidad + Utilidades.SEPARADOR_LOG + NombreServicio + Utilidades.SEPARADOR_LOG + Mensaje;

            if (TipoLog == Enumeradores.EnumTiposLog.Info)
            {
                _LogInterno.Info(cadena);
            }
            else if (TipoLog == Enumeradores.EnumTiposLog.Advertencia)
            {
                _LogInterno.Warn(cadena);
            }
            else if (TipoLog == Common.Enumeradores.EnumTiposLog.Error)
            {
                _LogInterno.Error(cadena);
            }
            else if (TipoLog == Enumeradores.EnumTiposLog.Fatal)
            {
                _LogInterno.Fatal(cadena);
            }
        }

        /// <summary>
        /// Método que registra un mensaje y tipo de log
        /// </summary>
        /// <param name="TipoLog"></param>
        /// <param name="NombreServicio"></param>
        /// <param name="Mensaje"></param>
        public void RegistrarLog(Enumeradores.EnumTiposLog TipoLog, string NombreServicio, string Mensaje)
        {
            string CodCompletoComunidad = string.Empty;

            if (!string.IsNullOrEmpty(IdComunidad) && !string.IsNullOrEmpty(CodigoComunidad))
                CodCompletoComunidad = IdComunidad + "-" + CodigoComunidad;

            string cadena = CodCompletoComunidad + Utilidades.SEPARADOR_LOG + NombreServicio + Utilidades.SEPARADOR_LOG + Mensaje;

            if (TipoLog == Enumeradores.EnumTiposLog.Info)
            {
                _LogInterno.Info(cadena);
            }
            else if (TipoLog == Enumeradores.EnumTiposLog.Advertencia)
            {
                _LogInterno.Warn(cadena);
            }
            else if (TipoLog == Common.Enumeradores.EnumTiposLog.Error)
            {
                _LogInterno.Error(cadena);
            }
            else if (TipoLog == Enumeradores.EnumTiposLog.Fatal)
            {
                _LogInterno.Fatal(cadena);
            }
        }




        /// <summary>
        /// Método que registra un mensaje y tipo de log
        /// </summary>
        /// <param name="TipoLog"></param>
        /// <param name="NombreServicio"></param>
        /// <param name="Mensaje"></param>
        public void RegistrarLog(Enumeradores.EnumTiposLog TipoLog, string NombreServicio, string Mensaje, Exception Error)
        {
            string CodCompletoComunidad = string.Empty;

            if (!string.IsNullOrEmpty(IdComunidad) && !string.IsNullOrEmpty(CodigoComunidad))
                CodCompletoComunidad = IdComunidad + "-" + CodigoComunidad;

            string cadena = CodCompletoComunidad + Utilidades.SEPARADOR_LOG + NombreServicio + Utilidades.SEPARADOR_LOG + Mensaje + Utilidades.SEPARADOR_LOG + Error.Message + Utilidades.SEPARADOR_LOG + Error.StackTrace.ToString();

            if (TipoLog == Enumeradores.EnumTiposLog.Info)
            {
                _LogInterno.Info(cadena);
            }
            else if (TipoLog == Enumeradores.EnumTiposLog.Advertencia)
            {
                _LogInterno.Warn(cadena);
            }
            else if (TipoLog == Common.Enumeradores.EnumTiposLog.Error)
            {
                _LogInterno.Error(cadena);
            }
            else if (TipoLog == Enumeradores.EnumTiposLog.Fatal)
            {
                _LogInterno.Fatal(cadena);
            }
        }

        /// <summary>
        /// Método que registra un mensaje y tipo de log
        /// </summary>
        /// <param name="TipoLog"></param>
        /// <param name="Mensaje"></param>
        public void RegistrarLog(Enumeradores.EnumTiposLog TipoLog, Exception Error)
        {
            string CodCompletoComunidad = string.Empty;

            if (!string.IsNullOrEmpty(IdComunidad) && !string.IsNullOrEmpty(CodigoComunidad))
                CodCompletoComunidad = IdComunidad + "-" + CodigoComunidad;

            string cadena = CodCompletoComunidad + Utilidades.SEPARADOR_LOG + NombreServicio + Utilidades.SEPARADOR_LOG + string.Empty + Utilidades.SEPARADOR_LOG + Error.Message + Utilidades.SEPARADOR_LOG + Error.StackTrace.ToString();

            if (TipoLog == Enumeradores.EnumTiposLog.Info)
            {
                _LogInterno.Info(cadena);
            }
            else if (TipoLog == Enumeradores.EnumTiposLog.Advertencia)
            {
                _LogInterno.Warn(cadena);
            }
            else if (TipoLog == Common.Enumeradores.EnumTiposLog.Error)
            {
                _LogInterno.Error(cadena);
            }
            else if (TipoLog == Enumeradores.EnumTiposLog.Fatal)
            {
                _LogInterno.Fatal(cadena);
            }
        }

        /// <summary>
        /// Método que registra un mensaje y tipo de log
        /// </summary>
        /// <param name="TipoLog"></param>
        /// <param name="Mensaje"></param>
        public void RegistrarLog(Enumeradores.EnumTiposLog TipoLog, string Mensaje, Exception Error)
        {
            string CodCompletoComunidad = string.Empty;

            if (!string.IsNullOrEmpty(IdComunidad) && !string.IsNullOrEmpty(CodigoComunidad))
                CodCompletoComunidad = IdComunidad + "-" + CodigoComunidad;

            string cadena = CodCompletoComunidad + Utilidades.SEPARADOR_LOG + NombreServicio + Utilidades.SEPARADOR_LOG + Mensaje + Utilidades.SEPARADOR_LOG + Error.Message + Utilidades.SEPARADOR_LOG + Error.StackTrace.ToString();

            if (TipoLog == Enumeradores.EnumTiposLog.Info)
            {
                _LogInterno.Info(cadena);
            }
            else if (TipoLog == Enumeradores.EnumTiposLog.Advertencia)
            {
                _LogInterno.Warn(cadena);
            }
            else if (TipoLog == Common.Enumeradores.EnumTiposLog.Error)
            {
                _LogInterno.Error(cadena);
            }
            else if (TipoLog == Enumeradores.EnumTiposLog.Fatal)
            {
                _LogInterno.Fatal(cadena);
            }
        }

        /// <summary>
        /// Método que registra un Log
        /// </summary>
        /// <param name="Logeador"></param>
        /// <param name="TipoLog"></param>
        /// <param name="Mensaje"></param>
        public void RegistrarLog(Logger Logeador, Enumeradores.EnumTiposLog TipoLog, string Mensaje)
        {
            string CodCompletoComunidad = string.Empty;

            if (!string.IsNullOrEmpty(IdComunidad) && !string.IsNullOrEmpty(CodigoComunidad))
                CodCompletoComunidad = IdComunidad + "-" + CodigoComunidad;

            string cadena = CodCompletoComunidad + Utilidades.SEPARADOR_LOG + NombreServicio + Utilidades.SEPARADOR_LOG + Mensaje;

            if (TipoLog == Enumeradores.EnumTiposLog.Info)
            {
                Logeador.Info(cadena);
            }
            else if (TipoLog == Enumeradores.EnumTiposLog.Advertencia)
            {
                Logeador.Warn(cadena);
            }
            else if (TipoLog == Common.Enumeradores.EnumTiposLog.Error)
            {
                Logeador.Error(cadena);
            }
            else if (TipoLog == Enumeradores.EnumTiposLog.Fatal)
            {
                Logeador.Fatal(cadena);
            }
        }

        /// <summary>
        /// Método que registra un mensaje y tipo de log
        /// </summary>
        /// <param name="TipoLog"></param>
        /// <param name="Mensaje"></param>
        public void RegistrarLog(Logger Logeador, Enumeradores.EnumTiposLog TipoLog, string Mensaje, Exception Error)
        {
            string CodCompletoComunidad = string.Empty;

            if (!string.IsNullOrEmpty(IdComunidad) && !string.IsNullOrEmpty(CodigoComunidad))
                CodCompletoComunidad = IdComunidad + "-" + CodigoComunidad;

            string cadena = CodCompletoComunidad + Utilidades.SEPARADOR_LOG + NombreServicio + Utilidades.SEPARADOR_LOG + Mensaje + Utilidades.SEPARADOR_LOG + Error.Message + Utilidades.SEPARADOR_LOG + Error.StackTrace.ToString();

            if (TipoLog == Enumeradores.EnumTiposLog.Info)
            {
                Logeador.Info(cadena);
            }
            else if (TipoLog == Enumeradores.EnumTiposLog.Advertencia)
            {
                Logeador.Warn(cadena);
            }
            else if (TipoLog == Common.Enumeradores.EnumTiposLog.Error)
            {
                Logeador.Error(cadena);
            }
            else if (TipoLog == Enumeradores.EnumTiposLog.Fatal)
            {
                Logeador.Fatal(cadena);
            }
        }

        /// <summary>
        /// Método que registra un Log
        /// </summary>
        /// <param name="Logeador"></param>
        /// <param name="TipoLog"></param>
        /// <param name="IdComunidad"></param>
        /// <param name="CodigoComunidad"></param>
        /// <param name="NombreServicio"></param>
        /// <param name="Mensaje"></param>
        /// <param name="StackTrace"></param>
        /// <param name="NumDte"></param>
        public void RegistrarLog(Logger Logeador, Enumeradores.EnumTiposLog TipoLog, string IdComunidad, string CodigoComunidad,  string NombreServicio, string Mensaje, string StackTrace)
        {
            string CodCompletoComunidad = string.Empty;

            if (!string.IsNullOrEmpty(IdComunidad) && !string.IsNullOrEmpty(CodigoComunidad))
                CodCompletoComunidad = IdComunidad + "-" + CodigoComunidad;

            string cadena = CodCompletoComunidad + Utilidades.SEPARADOR_LOG + NombreServicio + Utilidades.SEPARADOR_LOG + Mensaje + Utilidades.SEPARADOR_LOG + StackTrace;

            if (TipoLog == Enumeradores.EnumTiposLog.Info)
            {
                Logeador.Info(cadena);
            }
            else if (TipoLog == Enumeradores.EnumTiposLog.Advertencia)
            {
                Logeador.Warn(cadena);
            }
            else if (TipoLog == Common.Enumeradores.EnumTiposLog.Error)
            {
                Logeador.Error(cadena);
            }
            else if (TipoLog == Enumeradores.EnumTiposLog.Fatal)
            {
                Logeador.Fatal(cadena);
            }
        }


        /// <summary>
        /// Método que registra un Log
        /// </summary>
        /// <param name="TipoLog"></param>
        /// <param name="IdComunidad"></param>
        /// <param name="CodigoComunidad"></param>
        /// <param name="NombreServicio"></param>
        /// <param name="Mensaje"></param>
        /// <param name="StackTrace"></param>
        /// <param name="NumDte"></param>
        public void RegistrarLog(Enumeradores.EnumTiposLog TipoLog, string IdComunidad, string CodigoComunidad,  string NombreServicio, string Mensaje, string StackTrace)
        {
            string CodCompletoComunidad = string.Empty;

            if (!string.IsNullOrEmpty(IdComunidad) && !string.IsNullOrEmpty(CodigoComunidad))
                CodCompletoComunidad = IdComunidad + "-" + CodigoComunidad;

            string cadena = CodCompletoComunidad + Utilidades.SEPARADOR_LOG + NombreServicio + Utilidades.SEPARADOR_LOG + Mensaje + Utilidades.SEPARADOR_LOG + StackTrace;

            if (TipoLog == Enumeradores.EnumTiposLog.Info)
            {
                _LogInterno.Info(cadena);
            }
            else if (TipoLog == Enumeradores.EnumTiposLog.Advertencia)
            {
                _LogInterno.Warn(cadena);
            }
            else if (TipoLog == Common.Enumeradores.EnumTiposLog.Error)
            {
                _LogInterno.Error(cadena);
            }
            else if (TipoLog == Enumeradores.EnumTiposLog.Fatal)
            {
                _LogInterno.Fatal(cadena);
            }
        }

        /// <summary>
        /// Método que registra un Log
        /// </summary>
        /// <param name="TipoLog"></param>
        /// <param name="IdComunidad"></param>
        /// <param name="CodigoComunidad"></param>
        /// <param name="NombreServicio"></param>
        /// <param name="Mensaje"></param>
        public void RegistrarLog(Enumeradores.EnumTiposLog TipoLog, string IdComunidad, string CodigoComunidad,  string NombreServicio, string Mensaje)
        {
            string CodCompletoComunidad = string.Empty;

            if (!string.IsNullOrEmpty(IdComunidad) && !string.IsNullOrEmpty(CodigoComunidad))
                CodCompletoComunidad = IdComunidad + "-" + CodigoComunidad;

            string cadena = CodCompletoComunidad + Utilidades.SEPARADOR_LOG + NombreServicio + Utilidades.SEPARADOR_LOG + Mensaje + Utilidades.SEPARADOR_LOG + StackTrace;

            if (TipoLog == Enumeradores.EnumTiposLog.Info)
            {
                _LogInterno.Info(cadena);
            }
            else if (TipoLog == Enumeradores.EnumTiposLog.Advertencia)
            {
                _LogInterno.Warn(cadena);
            }
            else if (TipoLog == Common.Enumeradores.EnumTiposLog.Error)
            {
                _LogInterno.Error(cadena);
            }
            else if (TipoLog == Enumeradores.EnumTiposLog.Fatal)
            {
                _LogInterno.Fatal(cadena);
            }
        }
              

        /// <summary>
        /// Método que registra un mensaje y tipo de log
        /// </summary>
        /// <param name="TipoLog"></param>
        /// <param name="Mensaje"></param>
        public void RegistrarLog(Logger Logeador, Enumeradores.EnumTiposLog TipoLog, Exception Error)
        {
            string CodCompletoComunidad = string.Empty;

            if (!string.IsNullOrEmpty(IdComunidad) && !string.IsNullOrEmpty(CodigoComunidad))
                CodCompletoComunidad = IdComunidad + "-" + CodigoComunidad;

            string cadena = CodCompletoComunidad + Utilidades.SEPARADOR_LOG + NombreServicio + Utilidades.SEPARADOR_LOG + string.Empty + Utilidades.SEPARADOR_LOG + Error.Message + Utilidades.SEPARADOR_LOG + Error.StackTrace.ToString();

            if (TipoLog == Enumeradores.EnumTiposLog.Info)
            {
                Logeador.Info(cadena);
            }
            else if (TipoLog == Enumeradores.EnumTiposLog.Advertencia)
            {
                Logeador.Warn(cadena);
            }
            else if (TipoLog == Common.Enumeradores.EnumTiposLog.Error)
            {
                Logeador.Error(cadena);
            }
            else if (TipoLog == Enumeradores.EnumTiposLog.Fatal)
            {
                Logeador.Fatal(cadena);
            }
        }

        /// <summary>
        /// Método que registra un Log
        /// </summary>
        /// <param name="Logeador"></param>
        /// <param name="TipoLog"></param>
        /// <param name="IdComunidad"></param>
        /// <param name="CodigoComunidad"></param>
        /// <param name="NombreServicio"></param>
        /// <param name="Mensaje"></param>
        /// <param name="StackTrace"></param>
        /// <param name="NumDte"></param>
        public void RegistrarLog(Logger Logeador, Enumeradores.EnumTiposLog TipoLog, string IdComunidad, string CodigoComunidad,  string NombreServicio, Exception Error)
        {
            string CodCompletoComunidad = string.Empty;

            if (!string.IsNullOrEmpty(IdComunidad) && !string.IsNullOrEmpty(CodigoComunidad))
                CodCompletoComunidad = IdComunidad + "-" + CodigoComunidad;

            string cadena = CodCompletoComunidad + Utilidades.SEPARADOR_LOG + NombreServicio + Utilidades.SEPARADOR_LOG + string.Empty + Utilidades.SEPARADOR_LOG + Error.Message + Utilidades.SEPARADOR_LOG + Error.StackTrace.ToString();

            if (TipoLog == Enumeradores.EnumTiposLog.Info)
            {
                Logeador.Info(cadena);
            }
            else if (TipoLog == Enumeradores.EnumTiposLog.Advertencia)
            {
                Logeador.Warn(cadena);
            }
            else if (TipoLog == Common.Enumeradores.EnumTiposLog.Error)
            {
                Logeador.Error(cadena);
            }
            else if (TipoLog == Enumeradores.EnumTiposLog.Fatal)
            {
                Logeador.Fatal(cadena);
            }
        }

        /// <summary>
        /// ATR: Registra log considerando Inner Exceptions.
        /// </summary>
        /// <param name="tipoLog"></param>
        /// <param name="exObject"></param>
        /// <param name="datosEmpresa"></param>
        public void RegistrarLog(Logger objLogger, Enumeradores.EnumTiposLog tipoLog, Exception exObject, string userMessage, params string[] datosEmpresa)
        {
            var finalMessage = string.Empty;

            if (!object.ReferenceEquals(exObject, null) && (!object.ReferenceEquals(objLogger, null)))
            {
                if (datosEmpresa.Any() && (!datosEmpresa.Count().Equals(0)))
                {
                    var finalRut = (string.IsNullOrEmpty(datosEmpresa[0]) && string.IsNullOrEmpty(datosEmpresa[0]))
                               ? string.Empty
                               : string.Format("{0}-{1}", datosEmpresa[0], datosEmpresa[1]);

                    var innerException = (!object.ReferenceEquals(exObject.InnerException, null))
                                             ? string.Format("InnerException: {0}", exObject.InnerException.Message.ToString(CultureInfo.InvariantCulture))
                                             : "NoInnerException";

                    var userFinalMessage = !string.IsNullOrEmpty(userMessage)
                                                ? string.Format("Registro: [ '{0}' ]", userMessage) : string.Empty;

                    switch (datosEmpresa.Count())
                    {
                        //el 1 y el 2 son obligatorios ( IdComunidad ->  CodigoComunidad )
                        case 2:
                            finalMessage = userFinalMessage + Utilidades.SEPARADOR_LOG + finalRut + Utilidades.SEPARADOR_LOG + string.Empty +
                                Utilidades.SEPARADOR_LOG + exObject.Message + Utilidades.SEPARADOR_LOG +
                                innerException + Utilidades.SEPARADOR_LOG + exObject.StackTrace.ToString(CultureInfo.InvariantCulture);
                            break;

                        case 3:
                            finalMessage = userFinalMessage + Utilidades.SEPARADOR_LOG + finalRut + Utilidades.SEPARADOR_LOG + datosEmpresa[2] + Utilidades.SEPARADOR_LOG + string.Empty +
                                Utilidades.SEPARADOR_LOG + exObject.Message + Utilidades.SEPARADOR_LOG +
                                innerException + Utilidades.SEPARADOR_LOG + exObject.StackTrace.ToString(CultureInfo.InvariantCulture);
                            break;

                        case 4:
                            finalMessage = userFinalMessage + Utilidades.SEPARADOR_LOG + finalRut + Utilidades.SEPARADOR_LOG + datosEmpresa[2] + Utilidades.SEPARADOR_LOG + datosEmpresa[3] + Utilidades.SEPARADOR_LOG + string.Empty +
                                Utilidades.SEPARADOR_LOG + exObject.Message + Utilidades.SEPARADOR_LOG +
                                innerException + Utilidades.SEPARADOR_LOG + exObject.StackTrace.ToString(CultureInfo.InvariantCulture);
                            break;

                    }
                    switch (tipoLog)
                    {
                        case Enumeradores.EnumTiposLog.Info:
                            objLogger.Info(finalMessage);
                            break;

                        case Enumeradores.EnumTiposLog.Advertencia:
                            objLogger.Warn(finalMessage);
                            break;

                        case Enumeradores.EnumTiposLog.Fatal:
                            objLogger.Fatal(finalMessage);
                            break;

                        case Enumeradores.EnumTiposLog.Error:
                            objLogger.Error(finalMessage);
                            break;
                    }
                }
            }
        }
    }
}
