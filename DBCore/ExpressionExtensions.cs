using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace DBCore
{
    public static class ExpressionExtensions
    {
        public static IReadOnlyList<MemberInfo> GetMemberAccessList(this LambdaExpression memberAccessExpression)
        {
            var memberPaths = memberAccessExpression
                .MatchMemberAccessList((p, e) => e.MatchSimpleMemberAccess<MemberInfo>(p));

            if (memberPaths == null)
            {
                throw new ArgumentException("");
            }

            return memberPaths;
        }
        
        public static TMemberInfo MatchSimpleMemberAccess<TMemberInfo>(
            this Expression parameterExpression,
            Expression memberAccessExpression)
            where TMemberInfo : MemberInfo
        {
            var memberInfos = MatchMemberAccess<TMemberInfo>(parameterExpression, memberAccessExpression);

            return memberInfos?.Count == 1 ? memberInfos[0] : null;
        }
        
        private static IReadOnlyList<TMemberInfo> MatchMemberAccess<TMemberInfo>(
            this Expression parameterExpression,
            Expression memberAccessExpression)
            where TMemberInfo : MemberInfo
        {
            var memberInfos = new List<TMemberInfo>();

            MemberExpression memberExpression;
            var unwrappedExpression = RemoveTypeAs(RemoveConvert(memberAccessExpression));
            do
            {
                memberExpression = unwrappedExpression as MemberExpression;

                if (!(memberExpression?.Member is TMemberInfo memberInfo))
                {
                    return null;
                }

                memberInfos.Insert(0, memberInfo);

                unwrappedExpression = RemoveTypeAs(RemoveConvert(memberExpression.Expression));
            }
            while (unwrappedExpression != parameterExpression);

            return memberInfos;
        }
        
        public static Expression RemoveTypeAs(this Expression expression)
        {
            while (expression?.NodeType == ExpressionType.TypeAs)
            {
                expression = ((UnaryExpression)RemoveConvert(expression)).Operand;
            }

            return expression;
        }
        
        public static IReadOnlyList<TMemberInfo> MatchMemberAccessList<TMemberInfo>(
            this LambdaExpression lambdaExpression,
            Func<Expression, Expression, TMemberInfo> memberMatcher)
            where TMemberInfo : MemberInfo
        {
            var parameterExpression = lambdaExpression.Parameters[0];

            if (RemoveConvert(lambdaExpression.Body) is NewExpression newExpression)
            {
                var memberInfos
                    = (List<TMemberInfo>)newExpression
                        .Arguments
                        .Select(a => memberMatcher(a, parameterExpression))
                        .Where(p => p != null)
                        .ToList()!;

                return memberInfos.Count != newExpression.Arguments.Count ? null : memberInfos;
            }

            var memberPath = memberMatcher(lambdaExpression.Body, parameterExpression);

            return memberPath != null ? new[] { memberPath } : null;
        }
        
        private static Expression RemoveConvert(Expression expression)
        {
            if (expression is UnaryExpression unaryExpression
                && (expression.NodeType == ExpressionType.Convert
                    || expression.NodeType == ExpressionType.ConvertChecked))
            {
                return RemoveConvert(unaryExpression.Operand);
            }

            return expression;
        }
    }
}