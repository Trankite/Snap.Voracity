using Common.Source.Extension;
using System.Diagnostics;

namespace Common.Source.Web.Hoyolab.Takumi.Note
{
    public static class NoteRequestBuilderFactoryExtenison
    {
        [DebuggerStepThrough]
        public static NoteRequestBuilderFactory SetUid(this NoteRequestBuilderFactory builder, string uid)
        {
            return builder.Configure(builder.Uid = uid);
        }

        [DebuggerStepThrough]
        public static NoteRequestBuilderFactory SetServer(this NoteRequestBuilderFactory builder, string server)
        {
            return builder.Configure(builder.Server = server);
        }

        [DebuggerStepThrough]
        public static NoteRequestBuilderFactory SetUserRole(this NoteRequestBuilderFactory builder, HoyolabUserRole userRole)
        {
            return builder.SetServer(userRole.Server).SetUid(userRole.Uid);
        }
    }
}