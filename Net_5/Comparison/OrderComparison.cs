using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net_5.Comparison
{
    class OrderComparison
    {
        public static void Test()
        {
            #region ArraySort

            {
                // The static Array.Sort method works because System.String implements the IComparable interfaces:

                string[] colors = { "Green", "Red", "Blue" };
                Array.Sort(colors);
                #region  Sort soure code

                {
                    /*
                     public static void Sort(Array keys, Array? items, int index, int length, IComparer? comparer)
                        {
                          if (keys == null)
                            ThrowHelper.ThrowArgumentNullException(ExceptionArgument.keys);
                          if (keys.Rank != 1 || items != null && items.Rank != 1)
                            ThrowHelper.ThrowRankException(ExceptionResource.Rank_MultiDimNotSupported);
                          int lowerBound = keys.GetLowerBound(0);
                          if (items != null && lowerBound != items.GetLowerBound(0))
                            ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_LowerBoundsMustMatch);
                          if (index < lowerBound)
                            ThrowHelper.ThrowIndexArgumentOutOfRange_NeedNonNegNumException();
                          if (length < 0)
                            ThrowHelper.ThrowLengthArgumentOutOfRange_ArgumentOutOfRange_NeedNonNegNum();
                          if (keys.Length - (index - lowerBound) < length || items != null && index - lowerBound > items.Length - length)
                            ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidOffLen);
                          if (length <= 1)
                            return;
                          if (comparer == null)
                            comparer = (IComparer) Comparer.Default;
                          if (keys is object[] keys1)
                          {
                            object[] items1 = items as object[];
                            if (items == null || items1 != null)
                            {
                              new Array.SorterObjectArray(keys1, items1, comparer).Sort(index, length);
                              return;
                            }
                          }
                          if (comparer == Comparer.Default)
                          {
                            CorElementType typeOfElementType = keys.GetCorElementTypeOfElementType();
                            if (items == null || items.GetCorElementTypeOfElementType() == typeOfElementType)
                            {
                              int adjustedIndex = index - lowerBound;
                              switch (typeOfElementType)
                              {
                                case CorElementType.ELEMENT_TYPE_BOOLEAN:
                                case CorElementType.ELEMENT_TYPE_U1:
                                  GenericSort<byte>(keys, items, adjustedIndex, length);
                                  return;
                                case CorElementType.ELEMENT_TYPE_CHAR:
                                case CorElementType.ELEMENT_TYPE_U2:
                                  GenericSort<ushort>(keys, items, adjustedIndex, length);
                                  return;
                                case CorElementType.ELEMENT_TYPE_I1:
                                  GenericSort<sbyte>(keys, items, adjustedIndex, length);
                                  return;
                                case CorElementType.ELEMENT_TYPE_I2:
                                  GenericSort<short>(keys, items, adjustedIndex, length);
                                  return;
                                case CorElementType.ELEMENT_TYPE_I4:
                                  GenericSort<int>(keys, items, adjustedIndex, length);
                                  return;
                                case CorElementType.ELEMENT_TYPE_U4:
                                  GenericSort<uint>(keys, items, adjustedIndex, length);
                                  return;
                                case CorElementType.ELEMENT_TYPE_I8:
                                case CorElementType.ELEMENT_TYPE_I:
                                  GenericSort<long>(keys, items, adjustedIndex, length);
                                  return;
                                case CorElementType.ELEMENT_TYPE_U8:
                                case CorElementType.ELEMENT_TYPE_U:
                                  GenericSort<ulong>(keys, items, adjustedIndex, length);
                                  return;
                                case CorElementType.ELEMENT_TYPE_R4:
                                  GenericSort<float>(keys, items, adjustedIndex, length);
                                  return;
                                case CorElementType.ELEMENT_TYPE_R8:
                                  GenericSort<double>(keys, items, adjustedIndex, length);
                                  return;
                              }
                            }
                          }
                     */
                }

                #endregion
                foreach (string c in colors) Console.Write(c + " ");   // Blue Green Red
            }

            #endregion

            #region Interfaces

            {
                // The IComparable interfaces are defined as follows:
                //   public interface IComparable       { int CompareTo (object other); }
                //   public interface IComparable<in T> { int CompareTo (T other);      }

                Console.WriteLine("Beck".CompareTo("Anne"));       // 1
                Console.WriteLine("Beck".CompareTo("Beck"));       // 0
                Console.WriteLine("Beck".CompareTo("Chris"));      // -1
            }

            #endregion
        }
    }
}
