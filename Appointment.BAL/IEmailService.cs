﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appointment.BAL
{
    public interface IEmailService
    {
        void SendEmail(string toEmail, string subject, string body);
    }
}
