using System;
using System.Collections.Generic;
using System.Linq;
using Week3.Gun5Lab;

class Program
{
    static void Main()
    {
        LogService logService = new LogService();
        // 1.
        Console.WriteLine("--- Take Logs By Page number and Page size ---");
        var logsByPage = logService.GetLogsByPage(2, 0);
        foreach (var page in logsByPage)
        {
            Console.WriteLine(
                $"\tLog:{page.Id}\tDate:{page.CreatedAt}\tMessage:{page.Message}\tLog:{page.Severity}"
            );
        }
        // 2.
        Console.WriteLine("\n---Hic hata var mi ?---");
        Console.WriteLine($"\tHata bulundu mu ? {logService.HasAnyError()}");
        // 3.
        Console.WriteLine("\n--- Tum sistemler calisiyor mu?---");
        Console.WriteLine($"\tTum sistemler aktif mi? {logService.AreAllSystemsOperational()}");

        // 4.
        // Console.WriteLine("\n--- Deferred Execution Demo ---");
        // logService.DeferredExecutionDemo();

        // 5.
        Console.WriteLine("\n--- GetLatestError ---");
        Console.WriteLine($"\tSon hata: {logService.GetLatestError().Id}");

        // 6.
        Console.WriteLine("\n--- Son 1 saatteki uyarilar ---");
        var lastHourWarning = logService.GetLastHoursWarnings();
        foreach (var warning in lastHourWarning)
        {
            Console.WriteLine(
                $"\tLog:{warning.Id}\tDate:{warning.CreatedAt}\tMessage:{warning.Message}\tLog:{warning.Severity}"
            );
        }

        // 7. Test
        Console.WriteLine("\n--- Gorev 7: Log Sayilari (Severity) ---");
        try
        {
            var counts = logService.GetLogCountsBySeverity();
            foreach (var kvp in counts)
                Console.WriteLine($"\t{kvp.Key}: {kvp.Value} adet");
        }
        catch (NotImplementedException)
        {
            Console.WriteLine("\t[YAPILMADI] Gorev 7 bekleniyor...");
        }

        // 8. Test
        Console.WriteLine("\n--- Gorev 8: Sadece Mesajlar (Ilk 3) ---");
        try
        {
            var messages = logService.GetLogMessagesOnly();
            messages.Take(3).ToList().ForEach(m => Console.WriteLine($"\t{m}"));
        }
        catch (NotImplementedException)
        {
            Console.WriteLine("\t[YAPILMADI] Gorev 8 bekleniyor...");
        }

        // 9. Test
        Console.WriteLine("\n--- Gorev 9: Hizli Erisim (Dictionary) ---");
        try
        {
            var indexedLogs = logService.IndexLogsById();
            if (indexedLogs.TryGetValue(50, out var log50))
            {
                Console.WriteLine($"\tID 50 bulundu: {log50.Message}");
            }
        }
        catch (NotImplementedException)
        {
            Console.WriteLine("\t[YAPILMADI] Gorev 9 bekleniyor...");
        }

        // 10. Test
        Console.WriteLine("\n--- Gorev 10: Siralama (Severity -> Date) ---");
        try
        {
            var sortedLogs = logService.GetLogsSortedBySeverityAndDate();
            sortedLogs
                .Take(5)
                .ToList()
                .ForEach(l => Console.WriteLine($"\t{l.Severity} - {l.CreatedAt}"));
        }
        catch (NotImplementedException)
        {
            Console.WriteLine("\t[YAPILMADI] Gorev 10 bekleniyor...");
        }

        // 11. Test
        Console.WriteLine("\n--- Gorev 11: Lookup (Gruplama) ---");
        try
        {
            var lookup = logService.GetLogsLookupBySeverity();
            Console.WriteLine($"\tError Grubundaki Sayi: {lookup["Error"].Count()}");
            Console.WriteLine($"\tWarning Grubundaki Sayi: {lookup["Warning"].Count()}");
        }
        catch (NotImplementedException)
        {
            Console.WriteLine("\t[YAPILMADI] Gorev 11 bekleniyor...");
        }

        // 12. Test
        Console.WriteLine("\n--- Gorev 12: Tarih Araligi (Min/Max) ---");
        try
        {
            var range = logService.GetLogDateRange();
            Console.WriteLine($"\tEn Eski: {range.Oldest}");
            Console.WriteLine($"\tEn Yeni: {range.Newest}");
        }
        catch (NotImplementedException)
        {
            Console.WriteLine("\t[YAPILMADI] Gorev 12 bekleniyor...");
        }

        // 13. Test
        Console.WriteLine("\n--- Gorev 13: Performans (Gereksiz ToList) ---");
        try
        {
            var logs = logService.GetFirstFiveInfoLogsOptimized();
            Console.WriteLine($"\tAlinan Log Sayisi: {logs.Count}");
            logs.ForEach(l => Console.WriteLine($"\t- {l.Message}"));
        }
        catch (NotImplementedException)
        {
            Console.WriteLine("\t[YAPILMADI] Gorev 13 bekleniyor...");
        }

        // 14. Test
        Console.WriteLine("\n--- Gorev 14: Performans (Count Optimized) ---");
        try
        {
            var count = logService.CountLogsGreaterThanIdOptimized(50);
            Console.WriteLine($"\tID > 50 olan log sayisi: {count}");
        }
        catch (NotImplementedException)
        {
            Console.WriteLine("\t[YAPILMADI] Gorev 14 bekleniyor...");
        }

        // 15. Test
        Console.WriteLine("\n--- Gorev 15: Guvenli Arama (FirstOrDefault) ---");
        try
        {
            var log = logService.FindLogByIdSafe(99999); // Olmayan ID
            Console.WriteLine(
                $"\tSonuc (Null olmali): {(log == null ? "Null (Basarili)" : "Hata (Null degil)")}"
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\t[HATA] Exception firsatildi: {ex.Message}");
        }

        // 16. Test (BOSS BATTLE)
        Console.WriteLine("\n--- Gorev 16: Boss Battle (Raporlama) ---");
        try
        {
            var report = logService.GetMostFrequentError();
            Console.WriteLine($"\t{report}");
        }
        catch (NotImplementedException)
        {
            Console.WriteLine("\t[YAPILMADI] Gorev 16 bekleniyor...");
        }
    }
}
