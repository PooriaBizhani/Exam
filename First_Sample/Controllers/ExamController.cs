using First_Sample.Application.Services;
using First_Sample.Domain.Entities;
using First_Sample.Shared.Dtos.Answer;
using First_Sample.Shared.ViewModels.Answers;
using First_Sample.Shared.ViewModels.Questions;
using First_Sample.Shared.ViewModels.Users;
using Microsoft.AspNetCore.Mvc;

namespace First_Sample.Presentation.Controllers
{
    public class ExamController : Controller
    {
        private readonly QuestionService _questionService;
        private readonly AnswerService _answerService;

        public ExamController(QuestionService questionService, AnswerService answerService)
        {
            _questionService = questionService;
            _answerService = answerService;
        }

        public async Task<IActionResult> Start()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ButtonClick()
        {
            ViewBag.Message = "دکمه کلیک شد!";
            return RedirectToAction("Questions");
        }

        public async Task<IActionResult> Questions(int index = 1)
        {
            // تعداد کل سوالات را بگیرید
            var totalQuestions = await _questionService.GetTotelQuestionService();

            if (index < 1 || index > totalQuestions)
            {
                TempData["ErrorMessage"] = "ایندکس سوال باید بزرگتر از 0 و کمتر از یا مساوی تعداد کل سوالات باشد.";
            }
            var timerDuration = TimeSpan.FromMinutes(10);
            var startTime = DateTime.Now;

            // ذخیره زمان شروع تایمر در سشن
            HttpContext.Session.SetString("StartTime", startTime.ToString("o"));
            // سوال بر اساس ایندکس را دریافت کنید
            var question = await _questionService.GetByIdService(index);
            if (question == null)
            {
                return NotFound();
            }

            // پاسخ قبلی را در صورت وجود دریافت کنید
            var previousAnswer = await _answerService.GetAnswerByQuestionIdService(question.QuestionId);

            var model = new QuestionVM
            {
                Id = question.QuestionId,
                Questions = question.QuestionText,
                PreviousChoice = previousAnswer?.Choice
            };

            // ایندکس و تعداد کل سوالات را به ویو ارسال کنید
            ViewBag.Index = index;
            ViewBag.TotalQuestions = totalQuestions;

            return View(model);
        }



        [HttpGet("/GetQuestion/{id}")]
        public async Task<IActionResult> GetQuestion(int id)
        {
            var question = await _questionService.GetByIdService(id); // فرض کنید این متد در سرویس وجود دارد
            if (question == null)
                return NotFound();

            return Json(new { text = question.QuestionText });
        }

        [HttpPost]
        public async Task<IActionResult> SubmitAnswer([FromBody] AnswerVM model)
        {
            // Check timer
            var startTimeString = HttpContext.Session.GetString("StartTime");
            if (startTimeString == null)
            {
                return BadRequest("زمان شروع تایمر پیدا نشد.");
            }

            if (!DateTime.TryParse(startTimeString, out var startTime))
            {
                return BadRequest("زمان شروع تایمر نادرست است.");
            }

            var elapsedTime = DateTime.Now - startTime;
            var timerDuration = TimeSpan.FromMinutes(10);

            if (elapsedTime >= timerDuration)
            {
                return RedirectToAction("Start", "Exam"); // Redirect to start page if time is up
            }

            // Save the answer
            var answer = new AnswerDto
            {
                Choice = model.Choice,
                QuestionId = model.QuestionId,
            };

            await _answerService.AddService(answer);

            // Check if all questions have been answered
            var totalQuestions = await _questionService.GetTotelQuestionService();
            var answeredQuestions = await _answerService.GetAnsweredQuestionCountService(1); // Count of all answered questions

            if (answeredQuestions >= totalQuestions)
            {
                // Redirect to the "Results" page if all questions have been answered
                return RedirectToAction("Results", "Exam");
            }

            // Otherwise, proceed to the next question
            return Ok();
        }

        public async Task<IActionResult> Results()
        {
            // دریافت نتایج پاسخ‌های کاربر از سرویس
            var results = await _answerService.GetUserAnswersService();

            // بررسی و ایجاد پیام نتیجه آزمون
            var resultMessage = await _answerService.GetExamResultsService(results);

            // ارسال نتایج به ویو برای نمایش
            return View("Results", resultMessage);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateAnswer([FromBody] AnswerVM model)
        {
            var answer = await _answerService.GetAnswerByQuestionIdService(model.QuestionId);
            var answerDto = new AnswerDto
            {
                QuestionId = model.QuestionId,
                Choice = answer.Choice,
            };
            if (answer != null)
            {
                answer.Choice = model.Choice;
                await _answerService.UpdateService(answerDto);
            }
            return Ok();
        }
    }
}
