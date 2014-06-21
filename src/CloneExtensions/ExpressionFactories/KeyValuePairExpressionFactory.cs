﻿using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace CloneExtensions.ExpressionFactories
{
    class KeyValuePairExpressionFactory<T> : DeepShallowExpressionFactoryBase<T>
    {
        private ConstructorInfo _constructor;
        private Type _keyType;
        private Type _valueType;

        public KeyValuePairExpressionFactory(ParameterExpression source, Expression target, ParameterExpression flags, ParameterExpression initializers, LabelTarget returnLabel)
            : base(source, target, flags, initializers, returnLabel)
        {
            var type = typeof(T);
            _constructor = type.GetConstructors().FirstOrDefault(c => c.GetParameters().Length == 2);
            _keyType = type.GetGenericArguments()[0];
            _valueType = type.GetGenericArguments()[1];
        }

        public override bool AddNullCheck
        {
            get
            {
                return false;
            }
        }

        protected override Expression GetCloneExpression(Func<Type, Expression, Expression> getItemCloneExpression)
        {
            return
                Expression.Assign(
                    Target,
                    Expression.New(
                        _constructor,
                        getItemCloneExpression(_keyType, Expression.Property(Source, "Key")),
                        getItemCloneExpression(_valueType, Expression.Property(Source, "Value"))));
        }
    }
}