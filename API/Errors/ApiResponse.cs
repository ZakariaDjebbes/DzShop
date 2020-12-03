﻿using System;

namespace API.Errors
{
	public class ApiResponse
	{
		public int StatusCode { get; set; }
		public string Message { get; set; }

		public ApiResponse(int statusCode, string message = null)
		{
			StatusCode = statusCode;
			Message = message ?? GetDefaultMessageForStatus(StatusCode);
		}

		private string GetDefaultMessageForStatus(int statusCode)
		{
			return statusCode switch
			{
				400 => "A bad request, you have made",
				401 => "Authorized, you are not",
				404 => "Resource found, it was not",
				500 => "Errors are the path to the dark side. Erros lead to anger. Anger leads to hate" +
				" hate leads to career change",
				_ => null
			};
		}
	}
}