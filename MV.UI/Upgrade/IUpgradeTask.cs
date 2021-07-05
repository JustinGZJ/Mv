using Mv.Core.JsonObjects;
using System;
using System.Threading.Tasks;

namespace Mv.Ui.Core.Upgrade
{
    public interface IUpgradeTask
    {
        Version CurrentVersion { get; }

        string Name { get; }

        Task<bool> UpdateAsync(AppMetadata metadata);
    }
}
