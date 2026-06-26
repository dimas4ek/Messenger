using Application.DTO;
using Client.Properties;

namespace Client.Utils;

public static class ImageHelper
{
    public static Image GetAvatar(ImageInfo? image)
    {
        if (image?.Data == null) return Resources.DefaultAvatarImage;

        using var ms = new MemoryStream(image.Data);
        return Image.FromStream(ms);
    }
    
    // Открывает системный диалог выбора файла изображения
    // в отдельном STA-потоке и возвращает путь к выбранному файлу.
    //
    // Почему используется StaTaskScheduler:
    // OpenFileDialog требует поток с ApartmentState.STA.
    // Если вызвать диалог из обычного Task.Run / ThreadPool,
    // поток будет MTA и диалог может завершиться ошибкой.
    //
    // Почему используется Task.Factory.StartNew:
    // Позволяет указать собственный TaskScheduler,
    // чтобы задача выполнилась именно в STA-потоке.
    //
    // Результат:
    // - путь к выбранному файлу, если пользователь нажал OK
    // - null, если пользователь отменил выбор
    public static async Task<string?> OpenFile()
    {
        var file = await Task.Factory.StartNew(() =>
            {
                using var dialog = new OpenFileDialog();
                dialog.Filter = @"Images|*.jpg;*.jpeg;*.png;*.gif;*.bmp;*.webp";
                dialog.Title = Strings.ImageHelper_SelectImageTitle;

                return dialog.ShowDialog() == DialogResult.OK ? dialog.FileName : null;
            },
            CancellationToken.None,
            TaskCreationOptions.None,
            new StaTaskScheduler()
        );
        return file;
    }
}