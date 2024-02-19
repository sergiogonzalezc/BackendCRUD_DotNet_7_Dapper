using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalWebScraping.NLog
{
    public class Enumeradores
    {
        public enum Precio
        {
            Min = 0,
            Max = 99000
        }

        public enum TipoDescuento
        {
            Porcentual,
            FijoPesos
        }

        public enum TipoSalida
        {
            Texto,
            ClassCss
        }

        public enum PrmViaPago
        {
            D, // Deposito  
            PCE, // Pago contra entrega
            TC, // Tarjeta de crédito
            TD  // Tarjeta de Débito
        }

        public enum PrmEstadoOrden
        {
            N, //Anulada
            D, //Despachada
            K, //Devuelta            
            P, //Pendiente (Deposito)
            A, //Enviada a Transbank
            R, //Rechazada por Transbank
            C, //Creada
            X, //Aprobada por Transbank
            M, //Rechazada por Mac inválida
            E  //Rechazada por validación de OC
        }

        public enum EnumTiposLog
        {
            Info,
            Advertencia,
            Error,
            Fatal
        }

        public enum EnumTiposIconos
        {
            info,
            delete,
            deny,
            edit,
            ok,
            warning,
            none
        }

        /*
         * IdTipoLicencia	Descripcion
            CL-ADV	Licencia Cloud Avanzada
            CL-BAS	Licencia Cloud Básica
            CL-STA	Licencia Cloud Standar
            DEMO-CL-ADV	Licencia Demo Cloud Avanzada
            DEMO-SA-ADV	Licencia Demo Stand-Alone Avanzada
            DESA	Licencia de Desarrollo
            SA-ADV	Licencia Stand-Alone Avanzada
            SA-BAS	Licencia Stand-Alone Basica
            SA-STA	Licencia Stand-Alone Standar
         */

        public enum EnumTiposLicencia
        {
            CL_ADV,
            CL_BAS,
            CL_STA,
            DEMO_CL_ADV,
            DEMO_SA_ADV,
            DESA,
            SA_ADV,
            SA_BAS,
            SA_STA
        }

        public enum EnumMonedas
        {
            CLP,
            USD
        }

        /// <summary>
        /// Enumerador con los tipos de eventos a registrar en tabla de envío de correo
        /// </summary>
        public enum EnumTipoPlantilla : int
        {
            No_Aplica = -99,
            Recover_Pass_Customer = 1,
            Recover_Pass_Provider = 2,
            ChangePass_Client = 3,
            WebReservation_Customer = 4,
            WebReservation_Confirm_Prov = 42,
            WebReservation_Confirm_Admin = 43,
            NewCampaing_Req_Provider = 5,
            NewCampaing_Confirm_Admin = 41,
            NewCampaing_Confirm_Provider = 6,
            NewCampaing_Expired_Provider = 7,
            WebReservation_Expired_Customer = 8,
            WebReservation_Expired_Admin = 9,
            NewCampaing_Canceled_Provider = 10,
            WebReservation_Canceled_Customer = 11,
            WebReservation_Next_Notice_Customer = 12,
            WebReservation_Notified_Admin = 13,
            Birthday_Customer = 14,
            Suscription = 15,
            Anniversary_Operator_1_year = 16,
            Anniversary_Operator_2_year = 17,
            Anniversary_Operator_3_year = 18,
            Anniversary_Operator_5_year = 21,
            Anniversary_Operator_10_year = 19,
            WebPay_OK_Customer = 22,
            WebPay_Error_Customer = 23,
            New_Customer = 24,
            New_ProviderEval = 25,
            New_Provider = 26,
            New_TourGuide = 27,
            New_Campaign = 28,
            Contact_Customer = 29,
            PromoKickOff = 30,
            PromoNavidad = 31,
            PromoTempAlta = 32,
            PromoTempBaja = 33,
            PromoEspecial = 34,
            ChangePass_Provider = 35,
            New_UserVal = 36,
            PreRegisterProv = 37,
            Payment_TransferDirect = 38,
            Contact_CustomerCopy = 39,
            PreRegisterProvCopy = 40,
        }

        public enum EnumTipoAlgoritmo
        {
            MD5,
            SHA1,
            SHA256,
        }

        /// <summary>
        /// SGC 16-09-2019 - Enumerador que define el origen del pago (Proveedor. Pasajero)
        /// </summary>
        public enum EnumOrigenPago
        {
            PROVEEDOR,
            PASAJERO
        }

        /// <summary>
        /// SGC 16-03-2020 - Estados del pago
        /// </summary>
        public enum EnumEstadoPago
        {
            SIN_DEFINIR = -1,
            BORRADOR = 1,
            ENVIADO_A_PAGAR = 2,
            VALIDANDO_PAGO = 3,
            CONFIRMADO = 4,
            CANCELADO = 5,
            ANULADO = 6,
            PAGADO = 7
        }

        public enum EnumMessage
        {
            Succes,
            Error,
            Alert
        }

        /*
         *  '1', 'Clínica Dávila', '1'
            '2', 'Clínica Indisa', '1'
            '3', 'Clínica Santa María', '1'
            '4', 'Clínica Bio Bio', '1'
            '5', 'Red Salud UC Christus', '1'
            '6', 'Red Salud', '1'
*/

        public enum MedicalCenter : int
        {
            CLINICA_DAVILA = 1,
            CLINICA_INDISA = 2,
            CLINICA_SANTA_MARIA = 3,
            CLINICA_BIO_BIO = 4,
            RED_SALUD_UC_CHRISTUS = 5,
            RED_SALUD = 6
        }

        public enum StatusUser
        {
            UserDoesNotExist,
            UserDisabled,
            UserDisabledFactoring,
            WrongPassword,
            ChangePass,
            ChangePassFirstLogin,
            ValidUser,
            UnauthorizedUser,
            HasUser
        }
    }
}