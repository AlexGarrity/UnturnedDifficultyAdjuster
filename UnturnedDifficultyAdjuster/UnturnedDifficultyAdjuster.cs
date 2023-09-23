using System;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using OpenMod.API.Eventing;
using OpenMod.API.Plugins;
using OpenMod.Unturned.Plugins;
using UnturnedDifficultyAdjuster.Config;

[assembly: PluginMetadata("UnturnedDifficultyAdjuster", DisplayName = "Unturned Difficulty Adjuster", Author = "Alex Garrity")]

namespace UnturnedDifficultyAdjuster
{
    public class UnturnedDifficultyAdjuster : OpenModUnturnedPlugin
    {
        private readonly IConfiguration m_Configuration;
        private readonly IEventBus m_eventBus;
        private readonly ILogger<UnturnedDifficultyAdjuster> m_logger;
        private readonly IStringLocalizer m_StringLocalizer;

        public UnturnedDifficultyAdjuster(
            IConfiguration configuration,
            IStringLocalizer stringLocalizer,
            ILogger<UnturnedDifficultyAdjuster> mLogger,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            m_Configuration = configuration;
            m_StringLocalizer = stringLocalizer;
            m_logger = mLogger;
        }

        protected override async UniTask OnLoadAsync()
        {
            // await UniTask.SwitchToMainThread(); uncomment if you have to access Unturned or UnityEngine APIs
            m_logger.LogInformation(m_StringLocalizer["plugin_events:plugin_start"]);

            // await UniTask.SwitchToThreadPool(); // you can switch back to a different thread

            UnturnedDifficultyAdjusterConfig.ParseConfig(m_Configuration, m_logger);
        }

        protected override async UniTask OnUnloadAsync()
        {
            // await UniTask.SwitchToMainThread(); uncomment if you have to access Unturned or UnityEngine APIs
            m_logger.LogInformation(m_StringLocalizer["plugin_events:plugin_stop"]);
        }
    }
}