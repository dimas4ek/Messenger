namespace Client;

/// <summary>
///     <para>
///         Планировщик задач, который запускает каждую Task в отдельном потоке
///         с установленным режимом STA (Single-Threaded Apartment).
///     </para>
///     STA требуется для работы многих COM-компонентов Windows,
///     включая WinForms/WPF диалоги:
///     <list type="bullet">
///         <item>
///             <description>
///                 <see cref="OpenFileDialog" />
///             </description>
///         </item>
///         <item>
///             <description>
///                 <see cref="SaveFileDialog" />
///             </description>
///         </item>
///         <item>
///             <description>Clipboard access</description>
///         </item>
///         <item>
///             <description>Drag-and-drop operations</description>
///         </item>
///         <item>
///             <description>Other legacy Win32 / COM UI APIs</description>
///         </item>
///     </list>
///     <para>
///         Если вызвать такие компоненты из обычного ThreadPool потока
///         (MTA по умолчанию), можно получить исключение:
///         "Current thread must be set to single thread apartment (STA)".
///     </para>
///     Используется, когда нужно выполнить UI-зависимую операцию
///     асинхронно, не блокируя основной поток приложения.
/// </summary>
public class StaTaskScheduler : TaskScheduler
{
    /// <summary>
    ///     <para>Помещает задачу в очередь выполнения.</para>
    ///     Для каждой задачи создаётся отдельный фоновый STA-поток.
    /// </summary>
    protected override void QueueTask(Task task)
    {
        var thread = new Thread(() => TryExecuteTask(task));
        thread.SetApartmentState(ApartmentState.STA);
        thread.IsBackground = true;
        thread.Start();
    }

    /// <summary>
    ///     <para>Запрещает встроенное (inline) выполнение задачи</para>
    ///     в текущем потоке, чтобы гарантировать запуск именно в STA-потоке.
    /// </summary>
    protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
    {
        return false;
    }

    /// <summary>
    ///     <para>Возвращает список запланированных задач.</para>
    ///     Здесь не используется, так как очередь не хранится.
    /// </summary>
    protected override IEnumerable<Task> GetScheduledTasks()
    {
        return [];
    }
}