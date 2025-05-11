using Event_ui.DTOs;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using System.Net;

namespace Event_ui.Util
{
    public class ErrorsHandler
    {
        public  void HandleErrorResponse(HttpResponseMessage response, string errorResponse, ITempDataDictionary TempData)
        {
            try
            {
                var problemDetails = System.Text.Json.JsonSerializer.Deserialize<ProblemDetailsResponse>(errorResponse,
                    new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (problemDetails?.Errors != null && problemDetails.Errors.Count > 0)
                {
                    TempData["ValidationErrors"] = System.Text.Json.JsonSerializer.Serialize(problemDetails.Errors);

                    var formattedErrors = new List<string>();
                    foreach (var kvp in problemDetails.Errors)
                    {
                        string fieldName = string.IsNullOrEmpty(kvp.Key) ? "General" : kvp.Key;
                        foreach (var error in kvp.Value)
                        {
                            formattedErrors.Add($"{fieldName}: {error}");
                        }
                    }

                    if (formattedErrors.Count > 0)
                    {
                        TempData["FormattedErrors"] = JsonConvert.SerializeObject(formattedErrors);
                    }
                }
                else if (!string.IsNullOrEmpty(problemDetails?.Title))
                {
                    TempData["ErrorMessage"] = problemDetails.Title;
                }
                else
                {
                    TempData["ErrorMessage"] = response.StatusCode switch
                    {
                        HttpStatusCode.Unauthorized => "You are not authorized to perform this action.",
                        HttpStatusCode.NotFound => "The requested resource was not found.",
                        HttpStatusCode.BadRequest => "The request was invalid.",
                        HttpStatusCode.Conflict => "The request could not be completed due to a conflict.",
                        HttpStatusCode.UnprocessableEntity => "Validation failed. Please check your input.",
                        _ => "An error occurred while processing your request."
                    };
                }
            }
            catch
            {
                TempData["ErrorMessage"] = response.StatusCode switch
                {
                    HttpStatusCode.Unauthorized => "You are not authorized to perform this action.",
                    HttpStatusCode.NotFound => "The requested resource was not found.",
                    HttpStatusCode.BadRequest => "The request was invalid.",
                    HttpStatusCode.Conflict => "The request could not be completed due to a conflict.",
                    HttpStatusCode.UnprocessableEntity => "Validation failed. Please check your input.",
                    _ => "An error occurred while processing your request."
                };
            }
        }
        public  void HandleGeneralErrorResponse(HttpResponseMessage response, string errorResponse, ITempDataDictionary TempData)
        {
            try
            {
                var problemDetails = System.Text.Json.JsonSerializer.Deserialize<ApiErrorResponse>(errorResponse,
                    new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });


                if (!string.IsNullOrEmpty(problemDetails?.Message))
                {
                    TempData["ErrorMessage"] = problemDetails.Message;
                }
                else
                {
                    TempData["ErrorMessage"] = response.StatusCode switch
                    {
                        HttpStatusCode.Unauthorized => "You are not authorized to perform this action.",
                        HttpStatusCode.NotFound => "The requested resource was not found.",
                        HttpStatusCode.BadRequest => "The request was invalid.",
                        HttpStatusCode.Conflict => "The request could not be completed due to a conflict.",
                        HttpStatusCode.UnprocessableEntity => "Validation failed. Please check your input.",
                        _ => "An error occurred while processing your request."
                    };
                }
            }
            catch
            {
                TempData["ErrorMessage"] = response.StatusCode switch
                {
                    HttpStatusCode.Unauthorized => "You are not authorized to perform this action.",
                    HttpStatusCode.NotFound => "The requested resource was not found.",
                    HttpStatusCode.BadRequest => "The request was invalid.",
                    HttpStatusCode.Conflict => "The request could not be completed due to a conflict.",
                    HttpStatusCode.UnprocessableEntity => "Validation failed. Please check your input.",
                    _ => "An error occurred while processing your request."
                };
            }
        }
        public  void HandleGeneralErrorWithInjectionResponse(HttpResponseMessage response, string errorResponse, ModelStateDictionary ModelState)
        {
            try
            {
                var problemDetails = System.Text.Json.JsonSerializer.Deserialize<ApiErrorResponse>(errorResponse,
                    new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });


                if (!string.IsNullOrEmpty(problemDetails?.Message))
                {
                    ModelState.AddModelError("", problemDetails.Message); 
                }
                else
                {
                    ModelState.AddModelError("", response.StatusCode switch
                    {
                        HttpStatusCode.Unauthorized => "You are not authorized to perform this action.",
                        HttpStatusCode.NotFound => "The requested resource was not found.",
                        HttpStatusCode.BadRequest => "The request was invalid.",
                        HttpStatusCode.Conflict => "The request could not be completed due to a conflict.",
                        HttpStatusCode.UnprocessableEntity => "Validation failed. Please check your input.",
                        _ => "An error occurred while processing your request."
                    });
                }
            }
            catch
            {
                ModelState.AddModelError("", response.StatusCode switch
                {
                    HttpStatusCode.Unauthorized => "You are not authorized to perform this action.",
                    HttpStatusCode.NotFound => "The requested resource was not found.",
                    HttpStatusCode.BadRequest => "The request was invalid.",
                    HttpStatusCode.Conflict => "The request could not be completed due to a conflict.",
                    HttpStatusCode.UnprocessableEntity => "Validation failed. Please check your input.",
                    _ => "An error occurred while processing your request."
                });
            }
        }
        public  void InjectErrorMessages(ITempDataDictionary TempData, ModelStateDictionary ModelState)
        {
            if (TempData.ContainsKey("ValidationErrors"))
            {
                var errorJson = TempData["ValidationErrors"] as string;
                if (!string.IsNullOrEmpty(errorJson))
                {
                    var errors = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(errorJson);
                    if (errors != null)
                    {
                        foreach (var kvp in errors)
                        {
                            foreach (var error in kvp.Value)
                            {
                                ModelState.AddModelError(kvp.Key, error);
                            }
                        }
                    }
                }
            }

            if (TempData.ContainsKey("FormattedErrors"))
            {
                var formattedErrorsJson = TempData["FormattedErrors"] as string;
                if (!string.IsNullOrEmpty(formattedErrorsJson))
                {
                    var formattedErrors = JsonConvert.DeserializeObject<List<string>>(formattedErrorsJson);
                    if (formattedErrors != null)
                    {
                        foreach (var error in formattedErrors)
                        {
                            ModelState.AddModelError(string.Empty, error);
                        }
                    }
                }
            }

            if (TempData.ContainsKey("ErrorMessage"))
            {
                ModelState.AddModelError(string.Empty, TempData["ErrorMessage"].ToString());
            }
        }
    }
}
