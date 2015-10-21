namespace Zanshin.Domain.Exceptions
{
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.Text;

    public sealed class ParameterNullException : ArgumentNullException
    {
        private readonly string parameterName;
        private Type failingParamterType;
        private readonly Type declaringType;
        private readonly MethodBase declaringMethod;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterNullException" /> class.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        public ParameterNullException(string parameterName)
            : base(parameterName, new ArgumentException(parameterName))
        {
            this.declaringMethod = new StackFrame(1).GetMethod();
            this.declaringType = this.DeclaringMethod.DeclaringType;

            if (string.IsNullOrEmpty(parameterName))
            {
                var sb = new StringBuilder();
                sb.Append(Common.ParameterNullExceptionNoName);
                foreach (var p in this.DeclaringMethod.GetParameters())
                {
                    sb.AppendFormat("{0}, ", p.Name);
                }
                this.parameterName = sb.ToString();
            }
            else
            {
                this.parameterName = parameterName;
            }
        }


        /// <summary>
        /// Gets the error message and the parameter name, or only the error message if no parameter name is set.
        /// </summary>
        public override string Message
        {
            get
            {
                var sb = new StringBuilder();

                sb.AppendFormat(Common.ParameterNullExceptionMessageOnMethod,
                    base.Message, this.DeclaringMethod.Name);

                var meInfo = (MethodInfo)this.DeclaringMethod;

                if (meInfo.DeclaringType != null)
                {
                    sb.AppendFormat(Common.ParameterNullExceptionMessageObjectIs, meInfo.DeclaringType.Name);
                }
                else
                {
                    sb.Append(Common.ParameterNullExceptionUnableToDetermineDeclaringType);
                }
                foreach (var p in this.DeclaringMethod.GetParameters())
                {
                    if (p.Name == this.ParameterName)
                    {
                        this.failingParamterType = p.ParameterType;
                        break;
                    }
                }

                if (ReferenceEquals(null, this.FailingParamterType))
                {
                    sb.Append(Common.ParameterNullExceptionUnableToDetermine);
                }
                else
                {
                    sb.AppendFormat(Common.ParameterNullExceptionMethodExpectedNonNull,
                        this.FailingParamterType.Name);
                }

                return sb.ToString();
            }
        }

        /// <summary>
        /// Gets the type of the declaring.
        /// </summary>
        /// <value>
        /// The type of the declaring.
        /// </value>
        public Type DeclaringType
        {
            get { return this.declaringType; }
        }

        /// <summary>
        /// Gets the type of the failing paramter.
        /// </summary>
        /// <value>
        /// The type of the failing paramter.
        /// </value>
        /// <remarks>This can be null!</remarks>
        public Type FailingParamterType
        {
            get { return this.failingParamterType; }
        }

        /// <summary>
        /// Gets the name of the parameter.
        /// </summary>
        /// <value>
        /// The name of the parameter.
        /// </value>
        public string ParameterName
        {
            get { return this.parameterName; }
        }

        /// <summary>
        /// Gets the declaring method.
        /// </summary>
        /// <value>
        /// The declaring method.
        /// </value>
        public MethodBase DeclaringMethod
        {
            get { return this.declaringMethod; }
        }
    }
}