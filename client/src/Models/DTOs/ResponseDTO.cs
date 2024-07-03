using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace client.src.Models.DTOs
{
    public class ResponseDTO<T>
    {
        public string? Message { get; set; }

        public DateTime Timestamp { get; set; }

        public int Elapsed { get; set; }

        public T? Data { get; set; }

        public string? Error { get; set; }

        public ResponseDTO()
        {
            Error = null;
        }

    }
}