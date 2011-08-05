namespace uppercut.tests.template.builder.settings
{
    public class DevTokenSettings
    {
        public static string Contents =
            @"<project>
    <property name=""environment"" value=""DEV"" />
    <property name=""server.files"" value=""appdevserver"" />
    
    <property name=""settings.separatedby.text"" value=""\\${server.files}\Apps-${environment}"" />
    <property name=""settings.separatedby.slash"" value=""\\${server.files}\${environment}\Archive"" />
    <property name=""settings.no.separation"" value=""${server.files}${environment}"" />
    <property name=""settings.with.nonexisting"" value=""${server.files}\${nonexistant.setting}"" />
    
</project>";
    }
}