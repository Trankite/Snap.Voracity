using Common.Source.Extension;
using Common.Source.Web.Response;
using System.Diagnostics.CodeAnalysis;

namespace Common.Source.Web.Hoyolab.Takumi.GameRole
{
    public class GameRoleResponse : ResponseWrapper<ListWrapper<GameRoleResponseWrapper>, HoyolabUserRole[]>
    {
        public override bool TryGetAnalyzedBody([NotNullWhen(true)] out HoyolabUserRole[]? analyedBody)
        {
            if (TryGetAnalyzedBody(out ListWrapper<GameRoleResponseWrapper>? Content))
            {
                return true.Configure(analyedBody = [.. Content.List.Select(Current => Current.GetUserRole())]);
            }
            return false.Configure(analyedBody = default);
        }
    }
}