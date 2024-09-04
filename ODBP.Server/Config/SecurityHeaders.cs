namespace Microsoft.AspNetCore.Builder
{
    public static class SecurityHeaders
    {
        public static IApplicationBuilder UseOdbpSecurityHeaders(this WebApplication app) => app.UseSecurityHeaders(x => x
            .AddDefaultSecurityHeaders()
            .AddCrossOriginOpenerPolicy(x =>
            {
                x.SameOrigin();
            })
            .AddCrossOriginEmbedderPolicy(x =>
            {
                x.RequireCorp();
            })
            .AddCrossOriginResourcePolicy(x =>
            {
                x.SameOrigin();
            })
            .AddContentSecurityPolicy(csp =>
            {
                csp.AddUpgradeInsecureRequests();
                csp.AddDefaultSrc().None();
                csp.AddConnectSrc().Self();
                csp.AddScriptSrc().Self();
                csp.AddStyleSrc().Self();
                csp.AddImgSrc().Self();
                csp.AddFontSrc().Self();
                csp.AddFrameAncestors().None();
                csp.AddFormAction().Self();
                csp.AddBaseUri().None();
            })
            .AddPermissionsPolicy(permissions =>
            {
                permissions.AddAccelerometer().None();
                permissions.AddAmbientLightSensor().None();
                permissions.AddAutoplay().None();
                permissions.AddCamera().None();
                permissions.AddEncryptedMedia().None();
                permissions.AddFullscreen().None();
                permissions.AddGeolocation().None();
                permissions.AddGyroscope().None();
                permissions.AddMagnetometer().None();
                permissions.AddMicrophone().None();
                permissions.AddMidi().None();
                permissions.AddPayment().None();
                permissions.AddPictureInPicture().None();
                permissions.AddSpeaker().None();
                permissions.AddSyncXHR().None();
                permissions.AddUsb().None();
                permissions.AddVR().None();
            }));
    }
}
