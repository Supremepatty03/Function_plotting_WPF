using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3_WPF
{
    public static class InputHandler
    {
        public static bool TryParse<T>(string input, out T result) where T : struct
        {
            var type = typeof(T);
            var ParsingMethod = type.GetMethod("TryParse", new[] { typeof(string), type.MakeByRefType() }); // создается массив типа Type, внутри которого параметры искомой функции
            if (ParsingMethod != null)
            {
                object[] parameters = { input, null }; // в null будет записан ответ (заранее вид результата не известен)
                bool outcome = (bool)ParsingMethod.Invoke(null, parameters); //null - объект, на котором вызывается метод
                result = outcome ? (T)parameters[1] : default; //явно приводим результат к типу Т в случае успеха
                return outcome;
            }
            result = default;
            return false;
        }
    }
}
