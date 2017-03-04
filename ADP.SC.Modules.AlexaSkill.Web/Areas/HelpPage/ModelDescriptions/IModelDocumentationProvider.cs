using System;
using System.Reflection;

namespace ADP.SC.Modules.AlexaSkill.Web.Areas.HelpPage.ModelDescriptions
{
    public interface IModelDocumentationProvider
    {
        string GetDocumentation(MemberInfo member);

        string GetDocumentation(Type type);
    }
}