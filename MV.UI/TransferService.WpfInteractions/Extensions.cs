using Mv.TransferService;
using System.Windows.Threading;

// ReSharper disable once CheckNamespace
namespace Mv.Ui.TransferService.WpfInteractions
{
    public static class Extensions
    {
        public static BindableDownloader ToBindable(this ITransferInfo<DownloadContext> @this, Dispatcher uiDispatcher = null)
        {
            return new BindableDownloader(@this, uiDispatcher);
        }

    }
}
