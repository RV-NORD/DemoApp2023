using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class RandomExtensions
    {

        public static DateTime NextDateTime(this Random random, DateTime from, DateTime to)
        {
            if (from >= to)
            {
                throw new Exception("Параметр \"from\" должен быть меньше параметра \"to\"!");
            }

            TimeSpan range = to - from;
            var randts = new TimeSpan((long)(random.NextDouble() * range.Ticks));
            return from + randts;
        }

        public static DateOnly NextDate(this Random random, DateTime from, DateTime to)
        {
            if (from >= to)
            {
                throw new Exception("Параметр \"from\" должен быть меньше параметра \"to\"!");
            }

            TimeSpan range = to - from;
            var randts = new TimeSpan((long)(random.NextDouble() * range.Ticks));
            return DateOnly.FromDateTime(from + randts);
        }
        public static byte[] NextBytes(this Random random, int Count)
        {
            if (Count < 0)
            {
                throw new ArgumentOutOfRangeException("Count", Count, "Размер должен быть больше нуля");
            }

            if (Count == 0)
            {
                return Array.Empty<byte>();
            }

            byte[] array = new byte[Count];
            random.NextBytes(array);
            return array;
        }



        //
        // Сводка:
        //     Случайный элемент из указанного набора вариантов (ссылка на элемент)
        //
        // Параметры:
        //   rnd:
        //     Генератор случайных чисел
        //
        //   items:
        //     Массив вариантов
        //
        // Параметры типа:
        //   T:
        //     Тип элементов
        //
        // Возврат:
        //     Ссылка на случайный элемент массива
        public static ref T NextItem<T>(this Random rnd, params T[] items)
        {
            return ref items[rnd.Next(items.Length)];
        }

        //
        // Сводка:
        //     Случайных элемент из списка
        //
        // Параметры:
        //   rnd:
        //     Генератор случайных чисел
        //
        //   items:
        //     Список элементов для выбора
        //
        // Параметры типа:
        //   T:
        //     Тип элементов для выбора
        //
        // Возврат:
        //     Случайный элемент из списка
        public static T NextItem<T>(this Random rnd, IList<T> items)
        {
            return items[rnd.Next(items.Count)];
        }

 

        //
        // Сводка:
        //     Массив целых неотрицательных случайных чисел
        //
        // Параметры:
        //   rnd:
        //     Датчик случайных чисел
        //
        //   Count:
        //     Размер массива
        //
        // Возврат:
        //     Массив целых неотрицательных случайных чисел
        public static int[] NextValues(this Random rnd, int Count)
        {
            int[] array = new int[Count];
            for (int i = 0; i < Count; i++)
            {
                array[i] = rnd.Next();
            }

            return array;
        }

        //
        // Сводка:
        //     Перечисление целых неотрицательных случайных чисел
        //
        // Параметры:
        //   rnd:
        //     Датчик случайных чисел
        //
        //   Count:
        //     Размер перечисления (если меньше 0, то бесконечное)
        //
        // Возврат:
        //     перечисление целых неотрицательных случайных чисел
        public static IEnumerable<int> NextValuesEnum(this Random rnd, int Count)
        {
            for (int i = 0; i < Count; i++)
            {
                yield return rnd.Next();
            }
        }

        //
        // Сводка:
        //     Массив целых неотрицательных случайных чисел ограниченный сверху (верхний предел
        //     не входит)
        //
        // Параметры:
        //   rnd:
        //     Датчик случайных чисел
        //
        //   Count:
        //     Размер массива
        //
        //   Max:
        //     Максимум (не входит)
        //
        // Возврат:
        //     Массив целых неотрицательных случайных чисел (верхний предел не входит)
        public static int[] NextValues(this Random rnd, int Count, int Max)
        {
            int[] array = new int[Count];
            for (int i = 0; i < Count; i++)
            {
                array[i] = rnd.Next(Max);
            }

            return array;
        }

        //
        // Сводка:
        //     Перечисление целых неотрицательных случайных чисел ограниченный сверху (верхний
        //     предел не входит)
        //
        // Параметры:
        //   rnd:
        //     Датчик случайных чисел
        //
        //   Count:
        //     Размер перечисления (если меньше 0, то бесконечное)
        //
        //   Max:
        //     Максимум (не входит)
        //
        // Возврат:
        //     Перечисление целых неотрицательных случайных чисел (верхний предел не входит)
        public static IEnumerable<int> NextValuesEnum(this Random rnd, int Count, int Max)
        {
            for (int i = 0; i < Count; i++)
            {
                yield return rnd.Next(Max);
            }
        }

        //
        // Сводка:
        //     Массив целых неотрицательных случайных чисел в заданном интервале (верхний предел
        //     не входит)
        //
        // Параметры:
        //   rnd:
        //     Датчик случайных чисел
        //
        //   Count:
        //     Размер массива
        //
        //   Min:
        //     Минимум
        //
        //   Max:
        //     Максимум (не входит)
        //
        // Возврат:
        //     Массив целых неотрицательных случайных чисел в заданном интервале (верхний предел
        //     не входит)
        public static int[] NextValues(this Random rnd, int Count, int Min, int Max)
        {
            int[] array = new int[Count];
            for (int i = 0; i < Count; i++)
            {
                array[i] = rnd.Next(Min, Max);
            }

            return array;
        }

        //
        // Сводка:
        //     Перечисление целых неотрицательных случайных чисел в заданном интервале (верхний
        //     предел не входит)
        //
        // Параметры:
        //   rnd:
        //     Датчик случайных чисел
        //
        //   Count:
        //     Размер перечисления (если меньше 0, то бесконечное)
        //
        //   Min:
        //     Минимум
        //
        //   Max:
        //     Максимум (не входит)
        //
        // Возврат:
        //     Перечисление целых неотрицательных случайных чисел в заданном интервале (верхний
        //     предел не входит)
        public static IEnumerable<int> NextValuesEnum(this Random rnd, int Count, int Min, int Max)
        {
            for (int i = 0; Count < 0 || i < Count; i++)
            {
                yield return rnd.Next(Min, Max);
            }
        }


        //
        // Сводка:
        //     Случайное значение true/false
        //
        // Параметры:
        //   rnd:
        //     Генератор случайных чисел
        public static bool NextBoolean(this Random rnd)
        {
            return rnd.Next(2) > 1;
        }

 
        //
        // Сводка:
        //     Случайных элемент из перечисленных вариантов
        //
        // Параметры:
        //   rnd:
        //     Датчик случайных чисел
        //
        //   count:
        //     Количество результатов выбора
        //
        //   variants:
        //     Перечисление вариантов выбора
        //
        // Параметры типа:
        //   T:
        //     Тип вариантов выбора
        //
        // Возврат:
        //     Последовательность случайных вариантов
        public static IEnumerable<T?> Next<T>(this Random rnd, int count, params T?[] variants)
        {
            if (rnd == null)
            {
                throw new ArgumentNullException("rnd");
            }

            if (variants == null)
            {
                throw new ArgumentNullException("variants");
            }

            int variants_count = variants.Length;
            for (int i = 0; i < count; i++)
            {
                yield return variants[rnd.Next(0, variants_count)];
            }
        }

        //
        // Сводка:
        //     Последовательность случайных целых чисел в указанном интервале
        //
        // Параметры:
        //   rnd:
        //     Датчик случайных чисел
        //
        //   min:
        //     Нижняя граница интервала (входит)
        //
        //   max:
        //     Верхняя граница интервала (не входит)
        //
        //   count:
        //     Размер выборки (если меньше 0), то бесконечная последовательность
        //
        // Возврат:
        //     Последовательность случайных целых чисел в указанном интервале
        public static IEnumerable<int> SequenceInt(this Random rnd, int min, int max, int count = -1)
        {
            if (rnd == null)
            {
                throw new ArgumentNullException("rnd");
            }

            if (count == 0)
            {
                yield break;
            }

            if (count < 0)
            {
                while (true)
                {
                    yield return rnd.Next(min, max);
                }
            }

            for (int i = 0; i < count; i++)
            {
                yield return rnd.Next(min, max);
            }
        }

        //
        // Сводка:
        //     Последовательность случайных вещественных чисел с равномерным распределением
        //     в интервале (0,1)
        //
        // Параметры:
        //   rnd:
        //     Датчик случайных чисел
        //
        //   count:
        //     Размер выборки (если меньше 0), то бесконечная последовательность
        //
        // Возврат:
        //     Последовательность случайных вещественных чисел в интервале (0,1)
        public static IEnumerable<double> SequenceDouble(this Random rnd, int count = -1)
        {
            if (rnd == null)
            {
                throw new ArgumentNullException("rnd");
            }

            if (count == 0)
            {
                yield break;
            }

            if (count < 0)
            {
                while (true)
                {
                    yield return rnd.NextDouble();
                }
            }

            for (int i = 0; count == -1 || i < count; i++)
            {
                yield return rnd.NextDouble();
            }
        }


    }
}
