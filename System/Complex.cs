namespace System
{
    using Text.RegularExpressions;

    /// <summary>
    ///   An implementation of a Complex number
    /// </summary>
    public struct Complex : IComparable, ICloneable
    {
        #region Fields
        /// <summary>
        /// </summary>
        public static readonly Complex Iota;

        /// <summary>
        /// </summary>
        public static readonly Complex Zero;

        /// <summary>
        /// </summary>
        public static readonly Complex One;

        /// <summary>
        /// </summary>
        public static readonly Complex MaxValue;

        /// <summary>
        /// </summary>
        public static readonly Complex MinValue;

        /// <summary>
        /// </summary>
        public static readonly Complex NaN;

        /// <summary>
        /// </summary>
        public static readonly Complex NegativeInfinity;

        /// <summary>
        /// </summary>
        public static readonly Complex PositiveInfinity;

        private const String realPart = "real";
        private const String imagPart = "imag";
        private const String reDecimal = @"[-+]?\s*\d*[.]?\d+"; //@"[-+]?\s*(\d+[.]?\d*|[.]\d+)";

        private static readonly String reComplex = String.Format(@"(?<{0}>{2})\s*(?<{1}>{2})\s*[i]", realPart, imagPart,
                                                                 reDecimal);

        private static readonly String reiComplex = String.Format(@"(?<{1}>{2})\s*[i]\s*(?<{0}>{2})\s*", realPart,
                                                                  imagPart, reDecimal);

        //private static String reBothComplex = String.Format(@"((?<{0}>{2})\s*(?<{1}>{2})\s*[i]) | ((?<{0}>{2})\s*[i](?<{1}>{2})\s*)", realPart, imagPart, reDecimal);

        private double _imag;
        private double _real;

        /// <summary>
        ///   Contains the real part of a complex number.
        /// </summary>
        public double Real
        {
            get { return _real; }
            set { _real = value; }
        }

        /// <summary>
        ///   Contains the imaginary part of a complex number.
        /// </summary>
        public double Imag
        {
            get { return _imag; }
            set { _imag = value; }
        }
        #endregion

        #region Properties
        /// <summary>
        /// </summary>
        public bool IsReal
        {
            get { return (0.0 == _imag); }
        }

        /// <summary>
        /// </summary>
        public bool IsImag
        {
            get { return (0.0 == _real); }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// </summary>
        static Complex()
        {
            Iota = new Complex(0, 1);
            Zero = new Complex();
            One = new Complex(1);
            MaxValue = new Complex(1.79769e+308, 1.79769e+308);
            MinValue = new Complex(-1.79769e+308, -1.79769e+308);
            NaN = 0.0/0.0;
            NegativeInfinity = -1.0/0.0;
            PositiveInfinity = 1.0/0.0;
        }

        /// <summary>
        ///   Complex number.
        /// </summary>
        /// <param name="real"> </param>
        /// <param name="imag"> </param>
        public Complex(double real, double imag)
        {
            _real = real;
            _imag = imag;
        }

        /// <summary>
        ///   Complex number with imaginary part = 0.
        /// </summary>
        /// <param name="real"> </param>
        public Complex(double real) : this(real, default(double))
        {
        }

        ///// <summary>
        ///// Complex number as (0, 0).
        ///// </summary>
        //public Complex() : this(0) { }

        /// <summary>
        ///   Copy Constructor
        /// </summary>
        /// <param name="complex"> </param>
        public Complex(Complex complex) : this(complex._real, complex._imag)
        {
        }

        /// <summary>
        ///   Complex number from String like "a+bi".
        /// </summary>
        /// <param name="complex"> String form of Complex </param>
        public Complex(String complex)
        {
            var regex = new Regex(reComplex);
            if (!regex.IsMatch(complex))
            {
                regex = new Regex(reiComplex);
                if (!regex.IsMatch(complex))
                    throw new FormatException();
            }
            var match = regex.Match(complex);
            try
            {
                _real = double.Parse(match.Groups[realPart].Value.Trim().Replace(" ", ""));
                _imag = double.Parse(match.Groups[imagPart].Value.Trim().Replace(" ", ""));
            }
            catch
            {
                _real = _imag = 0;
            }
        }
        #endregion

        #region Methods

        #region Statics
        /// <summary>
        /// </summary>
        /// <param name="complex"> </param>
        /// <returns> </returns>
        public static Match Test(String complex)
        {
            var regex = new Regex(reComplex);
            return regex.Match(complex);
        }

        /// <summary>
        /// </summary>
        /// <param name="complex"> </param>
        /// <returns> </returns>
        /// <exception cref="FormatException"></exception>
        public static Complex Parse(String complex)
        {
            var regex = new Regex(reComplex);
            if (!regex.IsMatch(complex))
                throw new FormatException();
            var match = regex.Match(complex);
            try
            {
                var real = double.Parse(match.Groups[realPart].Value.Trim().Replace(" ", ""));
                var imag = double.Parse(match.Groups[imagPart].Value.Trim().Replace(" ", ""));
                return new Complex(real, imag);
            }
            catch
            {
                return new Complex(0, 0);
            }
        }

        /// <summary>
        ///   Computes the conjugation of a complex number.
        /// </summary>
        /// <param name="complex"> </param>
        /// <returns> </returns>
        public static Complex Conjugate(Complex complex)
        {
            return new Complex(complex._real, -complex._imag);
        }

        /// <summary>
        ///   Calcs the absolute value of a complex number.
        /// </summary>
        /// <param name="complex"> </param>
        /// <returns> </returns>
        public static double Abs(Complex complex)
        {
            return Math.Sqrt(complex._real*complex._real + complex._imag*complex._imag);
        }

        /// <summary>
        ///   Complex's Inverse.
        /// </summary>
        /// <param name="complex"> </param>
        /// <returns> </returns>
        public static Complex Inverse(Complex complex)
        {
            return Conjugate(complex)/Math.Pow(Abs(complex), 2);
        }

        #region Trignometry
        /// <summary>
        ///   Complex's Sine.
        /// </summary>
        /// <param name="complex"> </param>
        /// <returns> </returns>
        public static Complex Sin(Complex complex)
        {
            return (Exp(Iota*complex) - Exp(-Iota*complex))/(2*Iota);
        }

        /// <summary>
        ///   Complex's Cosine.
        /// </summary>
        /// <param name="complex"> </param>
        /// <returns> </returns>
        public static Complex Cos(Complex complex)
        {
            return (Exp(Iota*complex) + Exp(-Iota*complex))/2;
        }

        /// <summary>
        ///   Complex's Tangent.
        /// </summary>
        /// <param name="complex"> </param>
        /// <returns> </returns>
        public static Complex Tan(Complex complex)
        {
            return Sin(complex)/Cos(complex);
        }

        /// <summary>
        ///   Complex's Cotangent.
        /// </summary>
        /// <param name="complex"> </param>
        /// <returns> </returns>
        public static Complex Cot(Complex complex)
        {
            return Cos(complex)/Sin(complex);
        }

        /// <summary>
        ///   Complex's Secant.
        /// </summary>
        /// <param name="complex"> </param>
        /// <returns> </returns>
        public static Complex Sec(Complex complex)
        {
            return 1/Cos(complex);
        }

        /// <summary>
        ///   Complex's Cosecant.
        /// </summary>
        /// <param name="complex"> </param>
        /// <returns> </returns>
        public static Complex Csc(Complex complex)
        {
            return 1/Sin(complex);
        }

        /// <summary>
        ///   Complex's Hyperbolic sine.
        /// </summary>
        /// <param name="complex"> </param>
        /// <returns> </returns>
        public static Complex Sinh(Complex complex)
        {
            return (Exp(complex) - Exp(-complex))/2;
        }

        /// <summary>
        ///   Complex's Hyperbolic cosine.
        /// </summary>
        /// <param name="complex"> </param>
        /// <returns> </returns>
        public static Complex Cosh(Complex complex)
        {
            return (Exp(complex) + Exp(-complex))/2;
        }

        /// <summary>
        ///   Complex's Hyperbolic tangent.
        /// </summary>
        /// <param name="complex"> </param>
        /// <returns> </returns>
        public static Complex Tanh(Complex complex)
        {
            return (Exp(2*complex) - 1)/(Exp(2*complex) + 1);
        }

        /// <summary>
        ///   Complex's Hyperbolic cotangent.
        /// </summary>
        /// <param name="complex"> </param>
        /// <returns> </returns>
        public static Complex Coth(Complex complex)
        {
            return (Exp(2*complex) + 1)/(Exp(2*complex) - 1);
        }

        /// <summary>
        ///   Complex's Hyperbolic secant.
        /// </summary>
        /// <param name="complex"> </param>
        /// <returns> </returns>
        public static Complex Sech(Complex complex)
        {
            return Inverse(Cosh(complex));
        }

        /// <summary>
        ///   Complex's Hyperbolic cosecant.
        /// </summary>
        /// <param name="complex"> </param>
        /// <returns> </returns>
        public static Complex Csch(Complex complex)
        {
            return Inverse(Sinh(complex));
        }
        #endregion

        #region Arithmatic
        public static Complex Pow(Complex c1, Complex c2)
        {
            return Exp(c2*Log(c1));
        }

        public static Complex Pow(double a, Complex b)
        {
            return Exp(b*Math.Log(a));
        }

        public static Complex Pow(Complex a, double b)
        {
            return Exp(b*Log(a));
        }

        /// <summary>
        ///   Complex square root.
        /// </summary>
        /// <param name="real"> </param>
        /// <returns> </returns>
        public static Complex Sqrt(double real)
        {
            if (real >= 0)
                return new Complex(Math.Sqrt(real));
            else
                return new Complex(0, Math.Sqrt(-real));
        }

        /// <summary>
        ///   Complex square root.
        /// </summary>
        /// <param name="complex"> </param>
        /// <returns> </returns>
        public static Complex Sqrt(Complex complex)
        {
            return Pow(complex, .5);
        }

        /// <summary>
        ///   Complex exponential function.
        /// </summary>
        /// <param name="complex"> </param>
        /// <returns> </returns>
        public static Complex Exp(Complex complex)
        {
            return new Complex(Math.Exp(complex._real)*Math.Cos(complex._imag),
                               Math.Exp(complex._real)*Math.Sin(complex._imag));
        }

        /// <summary>
        ///   Complex logarithm (ln).
        /// </summary>
        /// <param name="complex"> </param>
        /// <returns> </returns>
        public static Complex Log(Complex complex)
        {
            // Log[ |w| ] + ( Arg[ w ] + 2k*Pi) i            
            return new Complex(Math.Log(Abs(complex)), Arg(complex));
        }

        /// <summary>
        ///   Argument of the complex number.
        /// </summary>
        /// <param name="complex"> </param>
        /// <returns> </returns>
        public static double Arg(Complex complex)
        {
            if (complex._real < 0)
                if (complex._imag < 0)
                    return Math.Atan(complex._imag/complex._real) - Math.PI;
                else
                    return Math.PI - Math.Atan(-complex._imag/complex._real);
            else
                return Math.Atan(complex._imag/complex._real);
        }
        #endregion

        #endregion

        /// <summary>
        ///   Computes the conjugation.
        /// </summary>
        public Complex Conjugate()
        {
            return new Complex(_real, -_imag);
        }

        /// <summary>
        ///   Calcs the absolute value.
        /// </summary>
        public double Abs()
        {
            return Abs(this);
        }

        /// <summary>
        ///   Inverse.
        /// </summary>
        public Complex Inverse()
        {
            return Inverse(this);
        }

        /// <summary>
        /// </summary>
        /// <param name="complex"> </param>
        /// <returns> </returns>
        public bool Equals(Complex complex)
        {
            return this == complex;
        }

        /// <summary>
        /// </summary>
        /// <param name="complex"> </param>
        /// <returns> </returns>
        public bool NotEquals(Complex complex)
        {
            return this != complex;
        }

        public String ToString(String format)
        {
            if (this == Zero)
                return "0";
            else if (double.IsInfinity(_real) || double.IsInfinity(_imag))
                return (_real < 0 || _imag < 0 ? "-" : "") + "oo";
            else if (double.IsNaN(_real) || double.IsNaN(_imag))
                return "?";
            var reval = _real.ToString(format);
            var imval = _imag.ToString(format);
            imval = (imval == "1" ? "" : imval) + "i";
            if (IsReal) return reval;
            if (IsImag) return imval;
            //String reval = _real.ToString(format);
            //String imval = Math.Abs(_imag).ToString(format);
            //String sign = "";
            //if (_imag < 0)
            //    sign = "-";
            //else if (_real != 0)
            //    sign = "+";
            return reval + (_real != 0 && _imag > 0 ? "+" : "") + imval;
        }

        #region Overrided
        public override String ToString()
        {
            if (this == Zero)
                return "0";
            else if (double.IsInfinity(_real) || double.IsInfinity(_imag))
                return (_real < 0 || _imag < 0 ? "-" : "") + "oo";
            else if (double.IsNaN(_real) || double.IsNaN(_imag))
                return "?";
            var reval = _real.ToString();
            var imval = _imag.ToString();
            imval = (imval == "1" ? "" : imval) + "i";
            if (IsReal) return reval;
            if (IsImag) return imval;
            return reval + (_real != 0 && _imag > 0 ? "+" : "") + imval;
        }

        public override bool Equals(object obj)
        {
            //return obj is Complex ? Equals(obj) : base.Equals(obj);
            return ToString() == obj.ToString();
        }

        public override int GetHashCode()
        {
            return _real.GetHashCode() ^ _imag.GetHashCode() ^ base.GetHashCode();
        }
        #endregion

        #endregion

        #region Operators

        #region Conversions
        /// <summary>
        /// </summary>
        /// <param name="real"> </param>
        /// <returns> </returns>
        public static implicit operator Complex(double real)
        {
            return new Complex(real);
        }

        /// <summary>
        /// </summary>
        /// <param name="complex"> </param>
        /// <returns> </returns>
        public static implicit operator Complex(String complex)
        {
            return new Complex(complex);
        }
        #endregion

        #region Operations
        public static Complex operator -(Complex complex)
        {
            return new Complex(-complex._real, -complex._imag);
        }

        public static Complex operator +(Complex c1, Complex c2)
        {
            //if( null == c1 && null == c2 ) return default(Complex);
            //if( null == c1 ) return c2;
            //if( null == c2 ) return c1;
            return new Complex(c1._real + c2._real, c1._imag + c2._imag);
        }

        public static Complex operator +(Complex complex, double real)
        {
            return new Complex(complex._real + real, complex._imag);
        }

        public static Complex operator +(double real, Complex complex)
        {
            return new Complex(real + complex._real, complex._imag);
        }

        public static Complex operator -(Complex c1, Complex c2)
        {
            return c1 + (-c2);
        }

        public static Complex operator -(Complex complex, double real)
        {
            return complex + (-real);
        }

        public static Complex operator -(double real, Complex complex)
        {
            return (-real) + complex;
        }

        public static Complex operator *(Complex c1, Complex c2)
        {
            if (c1.IsReal && c2.IsReal)
                return new Complex(c1._real*c2._real);
            if (c1.IsImag && c2.IsImag)
                return new Complex(-c1._imag*c2._imag);
            if (c1.IsReal && c2.IsImag)
                return new Complex(0, c1._real*c2._imag);
            if (c1.IsImag && c2.IsReal)
                return new Complex(0, c1._imag*c2._real);
            return new Complex
                (
                c1._real*c2._real - c1._imag*c2._imag,
                c1._imag*c2._real + c1._real*c2._imag
                );
        }

        public static Complex operator *(Complex complex, double real)
        {
            return complex*new Complex(real);
        }

        public static Complex operator *(double real, Complex complex)
        {
            return new Complex(real)*complex;
        }

        public static Complex operator /(Complex c1, Complex c2)
        {
            return c1*Inverse(c2);
        }

        public static Complex operator /(Complex complex, double real)
        {
            return complex*(1/real);
        }

        public static Complex operator /(double real, Complex complex)
        {
            return real*Conjugate(complex)*(1/Math.Pow(Abs(complex), 2));
        }

        public static bool operator ==(Complex c1, Complex c2)
        {
            if (ReferenceEquals(c1, c2))
                return true;
            if (null == (object) c1 || null == (object) c2)
                return false;
            return c1._real == c2._real && c1._imag == c2._imag;
        }

        public static bool operator ==(Complex complex, double real)
        {
            return complex == new Complex(real);
        }

        public static bool operator ==(double real, Complex complex)
        {
            return new Complex(real) == complex;
        }

        public static bool operator !=(Complex c1, Complex c2)
        {
            return !(c1 == c2);
        }

        public static bool operator !=(Complex complex, double real)
        {
            return !(complex == real);
        }

        public static bool operator !=(double real, Complex complex)
        {
            return !(real == complex);
        }
        #endregion

        #endregion

        #region ICloneable Members
        object ICloneable.Clone()
        {
            return MemberwiseClone();
        }
        #endregion

        #region IComparable Members
        int IComparable.CompareTo(object obj)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        #endregion
    }
}