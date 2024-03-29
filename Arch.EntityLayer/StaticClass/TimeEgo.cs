﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arch.EntityLayer.StaticClass
{

    public static class TimeAgo
    {
        public static string GetTimeAgo(DateTime dateTime)
        {
            TimeSpan timeSince = DateTime.Now.Subtract(dateTime);

            if (timeSince.TotalMilliseconds < 1)
                return "şimdi";
            if (timeSince.TotalMinutes < 1)
                return "birkaç saniye önce";
            if (timeSince.TotalMinutes < 2)
                return "bir dakika önce";
            if (timeSince.TotalMinutes < 60)
                return string.Format("{0} dakika önce", timeSince.Minutes);
            if (timeSince.TotalMinutes < 120)
                return "bir saat önce";
            if (timeSince.TotalHours < 24)
                return string.Format("{0} saat önce", timeSince.Hours);
            if (timeSince.TotalDays < 2)
                return "dün";
            if (timeSince.TotalDays < 7)
                return string.Format("{0} gün önce", timeSince.Days);
            if (timeSince.TotalDays < 14)
                return "bir hafta önce";
            if (timeSince.TotalDays < 21)
                return "iki hafta önce";
            if (timeSince.TotalDays < 28)
                return "üç hafta önce";
            if (timeSince.TotalDays < 60)
                return "bir ay önce";
            if (timeSince.TotalDays < 365)
                return string.Format("{0} ay önce", Math.Round(timeSince.TotalDays / 30));
            if (timeSince.TotalDays < 730)
                return "bir yıl önce";

            return string.Format("{0} yıl önce", Math.Round(timeSince.TotalDays / 365));
        }
    }

}
