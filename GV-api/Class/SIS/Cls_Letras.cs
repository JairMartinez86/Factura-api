using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web;

namespace GV_api.Class.SIS
{
    public static class Cls_Letras
    {
        public static string NumeroALetras(this decimal numberAsString)
        {
            string dec;

            var entero = Convert.ToInt64(Math.Truncate(numberAsString));
            var decimales = Convert.ToInt32(Math.Round((numberAsString - entero) * 100, 2));
            if (decimales > 0)
            {
                //dec = " CORDOBA CON " + decimales.ToString() + "/100";
                dec = $" CORDOBA {decimales:0,0} /100";
            }
            //Código agregado por mí
            else
            {
                //dec = " CORDOBA CON " + decimales.ToString() + "/100";
                dec = $" CORDOBA {decimales:0,0} /100";
            }
            var res = NumeroALetras(Convert.ToDouble(entero)) + dec;
            return res;
        }
        [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
        private static string NumeroALetras(double value)
        {
            string num2Text; value = Math.Truncate(value);
            if (value == 0) num2Text = "Cero";
            else if (value == 1) num2Text = "Uno";
            else if (value == 2) num2Text = "Dos";
            else if (value == 3) num2Text = "Tres";
            else if (value == 4) num2Text = "Cuatro";
            else if (value == 5) num2Text = "Cinco";
            else if (value == 6) num2Text = "Seis";
            else if (value == 7) num2Text = "Siete";
            else if (value == 8) num2Text = "Ocho";
            else if (value == 9) num2Text = "Nueve";
            else if (value == 10) num2Text = "Diez";
            else if (value == 11) num2Text = "Once";
            else if (value == 12) num2Text = "Doce";
            else if (value == 13) num2Text = "Trece";
            else if (value == 14) num2Text = "Catorce";
            else if (value == 15) num2Text = "Quince";
            else if (value < 20) num2Text = "Dieci" + NumeroALetras(value - 10);
            else if (value == 20) num2Text = "Veinte";
            else if (value < 30) num2Text = "Veinti" + NumeroALetras(value - 20);
            else if (value == 30) num2Text = "Trainta";
            else if (value == 40) num2Text = "Cuarenta";
            else if (value == 50) num2Text = "Cincuenta";
            else if (value == 60) num2Text = "Sesenta";
            else if (value == 70) num2Text = "Setenta";
            else if (value == 80) num2Text = "Ochenta";
            else if (value == 90) num2Text = "Noventa";
            else if (value < 100) num2Text = NumeroALetras(Math.Truncate(value / 10) * 10) + " Y " + NumeroALetras(value % 10);
            else if (value == 100) num2Text = "Ciento";
            else if (value < 200) num2Text = "Ciento " + NumeroALetras(value - 100);
            else if ((value == 200) || (value == 300) || (value == 400) || (value == 600) || (value == 800)) num2Text = NumeroALetras(Math.Truncate(value / 100)) + "CIENTOS";
            else if (value == 500) num2Text = "Quinientos";
            else if (value == 700) num2Text = "Setecientos";
            else if (value == 900) num2Text = "Novecientos";
            else if (value < 1000) num2Text = NumeroALetras(Math.Truncate(value / 100) * 100) + " " + NumeroALetras(value % 100);
            else if (value == 1000) num2Text = "Mil";
            else if (value < 2000) num2Text = "Mil " + NumeroALetras(value % 1000);
            else if (value < 1000000)
            {
                num2Text = NumeroALetras(Math.Truncate(value / 1000)) + " Mil";
                if ((value % 1000) > 0)
                {
                    num2Text = num2Text + " " + NumeroALetras(value % 1000);
                }
            }
            else if (value == 1000000)
            {
                num2Text = "Un Millon";
            }
            else if (value < 2000000)
            {
                num2Text = "Un Millon " + NumeroALetras(value % 1000000);
            }
            else if (value < 1000000000000)
            {
                num2Text = NumeroALetras(Math.Truncate(value / 1000000)) + " Millones ";
                if ((value - Math.Truncate(value / 1000000) * 1000000) > 0)
                {
                    num2Text = num2Text + " " + NumeroALetras(value - Math.Truncate(value / 1000000) * 1000000);
                }
            }
            else if (value == 1000000000000) num2Text = "Un Billon";
            else if (value < 2000000000000) num2Text = "Un Billon " + NumeroALetras(value - Math.Truncate(value / 1000000000000) * 1000000000000);
            else
            {
                num2Text = NumeroALetras(Math.Truncate(value / 1000000000000)) + " Billones";
                if ((value - Math.Truncate(value / 1000000000000) * 1000000000000) > 0)
                {
                    num2Text = num2Text + " " + NumeroALetras(value - Math.Truncate(value / 1000000000000) * 1000000000000);
                }
            }
            return num2Text;
        }
    }
}