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
            // 本章描述.NET如何比较两个TYPE的前后，或大小
            #region 大部分BASE CLASS实现了ICOMPARABLE接口

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
                //   对于VALUE类型的值，用GENERIC的方法更快，因为不必BOXING,UNBOXING
                //   在RUNTIME时，OBJECT会转为具体的TYPE,找具体真正TYPE中的COMPARETO（）方法进行比较
                //   public interface IComparable<in T> { int CompareTo (T other);      }
                //STRING的COMPARETO有一堆OVERLOAD的方法，有的很复杂
                Console.WriteLine("Beck".CompareTo("Anne"));       // 1
                Console.WriteLine("Beck".CompareTo("Beck"));       // 0
                Console.WriteLine("Beck".CompareTo("Chris"));      // -1
            }

            #endregion

            #region Reloading <,>

            {
                // >,< 是STATIC BINDING,在编译时确定绑定TYPE,比较的一般是NUMERIC TYPE
                // 对于自定义CLASS,可以重写<,>, 但要满足三个条件，见NUTSHELL
                // Some types define < and > operators:
                bool after2010 = DateTime.Now > new DateTime(2010, 1, 1);

                // The string type doesn't overload these operators (for good reason):
               // bool error = "Beck" > "Anne";       // Compile-time error
            }

            #endregion
        }
    }
    //这个STRUCT继承三个接口
    public struct Note : IComparable<Note>, IEquatable<Note>, IComparable
    {
        int _semitonesFromA;  //BACKING FIELD
        public int SemitonesFromA => _semitonesFromA; // READ ONLY PROPERTY 

        public Note(int semitonesFromA)
        {
            _semitonesFromA = semitonesFromA;
        }

        public int CompareTo(Note other)            // Generic IComparable<T>
        {
            if (Equals(other)) return 0;    // Fail-safe check
            return _semitonesFromA.CompareTo(other._semitonesFromA);
        }
     

        //public int CompareTo(object obj)            // Nongeneric IComparable implicit 我加的
        //{
        //    throw new NotImplementedException();
        //}
        // https://stackoverflow.com/questions/1253266/why-explicit-implementation-of-a-interface-can-not-be-public
        //注意，EXPLICIT IMPLEMENT INTERFACE 不能用PUBLIC
        //而IMPLICIT的必须用，否则出错
        int IComparable.CompareTo(object other)     // Nongeneric IComparable这个接口例子中只EXPLICIT继承了一个
        {
            if (!(other is Note))
                throw new InvalidOperationException("CompareTo: Not a note");
            return CompareTo((Note)other);
        }

        //下面是OPERATOR OVERLOADING,因为NOTE是一种新的TYPE
        public static bool operator <(Note n1, Note n2)
            => n1.CompareTo(n2) < 0;

        public static bool operator >(Note n1, Note n2)
            => n1.CompareTo(n2) > 0;

        public bool Equals(Note other)    // for IEquatable<Note>
            => _semitonesFromA == other._semitonesFromA;

        public override bool Equals(object other)
        {
            if (!(other is Note)) return false;
            return Equals((Note)other);
        }
        // OVERRIDE了EQUALS,一般要重写OVERRIDE这个GETHASHCODE() 见NUTSHELL
        public override int GetHashCode()
            => _semitonesFromA.GetHashCode();
        //这个==应该不是OVERLOAD,而是直接定义
        public static bool operator ==(Note n1, Note n2)
            => n1.Equals(n2);

        public static bool operator !=(Note n1, Note n2)
            => !(n1 == n2);
    }

    class Dome : IComparable
    {
        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
