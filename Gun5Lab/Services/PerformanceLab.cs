using System;
using System.Collections.Generic;
using System.Linq;

namespace Week3.Gun5Lab
{
    public class SystemLog
    {
        public int Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string Severity { get; set; } = string.Empty; // Info, Warning, Error
    }

    public class LogService
    {
        private List<SystemLog> _logs;

        public LogService()
        {
            // Mock Data: 100 adet log olusturalim
            _logs = Enumerable
                .Range(1, 100)
                .Select(i => new SystemLog
                {
                    Id = i,
                    Message = $"Log entry {i}",
                    CreatedAt = DateTime.Now.AddMinutes(-(101 - i)),
                    Severity = i % 10 == 0 ? "Error" : (i % 5 == 0 ? "Warning" : "Info"),
                })
                .ToList();
        }

        // 1. SAYFALAMA (Paging): Skip ve Take Kullanimi
        public List<SystemLog> GetLogsByPage(int pageNumber, int pageSize)
        {
            Console.WriteLine($"\t{pageNumber} sayfasinin {pageSize} adet logu goruntuleniyor...");
            return _logs
                .OrderBy(l => l.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        // 2. VERIMLI KONTROL (ANY VS COUNT): Hic Hata Var mi ?
        public bool HasAnyError()
        {
            // KÖTÜ: return _logs.Where(l => l.Severity == "Error").Count() > 0;
            // KÖTÜ: return _logs.Count(l => l.Severity == "Error") > 0;
            return _logs.Any(l => l.Severity == "Error");
        }

        // 3. HEPSI UYGUN MU (ALL): Tum Sistemler Calisiyor mu?
        public bool AreAllSystemsOperational()
        {
            return _logs.All(l => l.Severity != "Fatal");
        }

        // 4. DEFERRED EXECUTION TUZAĞI (Demonstrasyon)
        public void DeferredExecutionDemo()
        {
            var query = _logs.Where(l => l.Severity == "Error"); // Sorgu tanımlandı, ÇALIŞMADI.

            Console.WriteLine("Sorgu tanımlandı.");

            _logs.Add(
                new SystemLog
                {
                    Id = 999,
                    Severity = "Error",
                    Message = "New Error",
                }
            ); // Listeye ekleme yapıldı

            Console.WriteLine($"Error Count: {query.Count()}"); // Sorgu BURADA çalışır. Yeni eklenen 999'u da görür!
        }

        // 5. GEREKSİZ MATERIALIZATION (ToList) KAÇINMA
        public SystemLog? GetLatestError()
        {
            // KÖTÜ: _logs.Where(...).ToList().FirstOrDefault(); -> Tüm hataları yeni bir listeye kopyalar (Heap Allocation), sonra ilkini alır.

            // İYİ: Sadece ilkini bulana kadar gezer, liste oluşturmaz.
            return _logs
                .Where(l => l.Severity == "Error")
                .OrderByDescending(l => l.CreatedAt)
                .FirstOrDefault();
        }

        // 6. Son 1 saat içinde oluşan "Warning" seviyesindeki logları getir.
        public List<SystemLog> GetLastHoursWarnings()
        {
            return _logs
                .Where(l => l.CreatedAt > DateTime.Now.AddHours(-1) && l.Severity == "Warning")
                .ToList();
        }

        // GÖREV 7: GROUP BY
        // Logları "Severity" alanına göre grupla ve her grupta kaç adet log olduğunu döndür.
        // İpucu: GroupBy() ve ToDictionary() kullan.
        public Dictionary<string, int> GetLogCountsBySeverity()
        {
            return _logs.GroupBy(g => g.Severity).ToDictionary(g => g.Key, g => g.Count());
            throw new NotImplementedException("Gorev 7 henuz yapilmadi!");
        }

        // GÖREV 8: SELECT (Projection)
        // Log nesnelerinin tamamını değil, sadece "Message" alanlarını içeren bir string listesi döndür.
        // İpucu: Select() kullan.
        public List<string> GetLogMessagesOnly()
        {
            return _logs.Select(l => l.Message).ToList();
            throw new NotImplementedException("Gorev 8 henuz yapilmadi!");
        }

        // GÖREV 9: DICTIONARY (Indexing)
        // Logları "Id" alanına göre indeksle.
        // Geriye Key=Id, Value=SystemLog olan bir Dictionary döndür.
        // İpucu: ToDictionary() kullan.
        public Dictionary<int, SystemLog> IndexLogsById()
        {
            return _logs.ToDictionary(g => g.Id, g => g);
        }

        // GÖREV 10: SIRALAMA (OrderBy & ThenBy)
        // Logları önce "Severity"ye göre (A-Z), sonra "CreatedAt" tarihine göre (Yeniden Eskiye) sırala.
        // İpucu: OrderBy().ThenByDescending() kullan.
        public List<SystemLog> GetLogsSortedBySeverityAndDate()
        {
            return _logs.OrderBy(l => l.Severity).ThenByDescending(l => l.CreatedAt).ToList();
            throw new NotImplementedException("Gorev 10 henuz yapilmadi!");
        }

        // GÖREV 11: LOOKUP (HashMap benzeri gruplama)
        // GroupBy ile Dictionary oluşturmak yerine, ILookup yapısı kullan.
        // Severity parametresine göre logları grupla.
        // İpucu: ToLookup() kullan.
        public ILookup<string, SystemLog> GetLogsLookupBySeverity()
        {
            return _logs.ToLookup(p => p.Severity);
            throw new NotImplementedException("Gorev 11 henuz yapilmadi!");
        }

        // GÖREV 12: AGGREGATION (Min / Max)
        // En eski ve En yeni log tarihini bulup geri döndür.
        // İpucu: Min() ve Max() kullan.
        public (DateTime Oldest, DateTime Newest) GetLogDateRange()
        {
            return (_logs.Min(p => p.CreatedAt), _logs.Max(p => p.CreatedAt));
        }

        // GÖREV 13: PERFORMANS (Gereksiz ToList)
        // "Info" olanları filtrele ve sadece ilk 5 tanesini geri dön.
        // DİKKAT: _logs.Where(...).ToList().Take(5) YAPMA! Bu tüm listeyi kopyalar.
        // ToList() metodunu EN SONDA çağır.
        public List<SystemLog> GetFirstFiveInfoLogsOptimized()
        {
            return _logs.Where(l => l.Severity == "Info").Take(5).ToList();
            throw new NotImplementedException("Gorev 13 (Optimize) henuz yapilmadi!");
        }

        // GÖREV 14: PERFORMANS (Count vs Where)
        // Verilen ID'den büyük olan logların sayısını bul.
        // DİKKAT: _logs.Where(l => l.Id > minId).Count() YAPMA! (İki tur atma)
        // Count() metodunun overload'ını kullan.
        public int CountLogsGreaterThanIdOptimized(int minId)
        {
            return _logs.Count(l => l.Id > minId);
            throw new NotImplementedException("Gorev 14 (Optimize) henuz yapilmadi!");
        }

        // GÖREV 15: GÜVENLİK & PERFORMANS (First vs FirstOrDefault)
        // Verilen ID'ye sahip logu bul. Yoksa null dönmeli. ASLA HATA FIRLATMAMALI.
        // DİKKAT: First() kullanırsan ve ID yoksa program "InvalidOperationException" hatası verir.
        // Try-Catch kullanmak yerine doğru LINQ metodunu (OrDefault) seç.
        public SystemLog? FindLogByIdSafe(int id)
        {
            return _logs.FirstOrDefault(l => l.Id == id);
            throw new NotImplementedException("Gorev 15 (Optimize) henuz yapilmadi!");
        }

        // BOSS BATTLE: Raporlama
        // Sadece "Error" olanları al, mesajlarına göre grupla, en çok tekrar edeni bul.
        // Format: "En Sık Hata: {Mesaj} ({Adet} kez)"
        public string GetMostFrequentError()
        {
            var mostFrequent = _logs
                .Where(l => l.Severity == "Error")
                .GroupBy(m => m.Message)
                .OrderByDescending(g => g.Count())
                .FirstOrDefault();

            if (mostFrequent == null)
                return "Hata bulunamadi.";
            // DÜZELTME 2: İstenen string formatına çevir
            return $"En Sık Hata: {mostFrequent.Key} ({mostFrequent.Count()} kez)";
            throw new NotImplementedException();
        }
    }
}
