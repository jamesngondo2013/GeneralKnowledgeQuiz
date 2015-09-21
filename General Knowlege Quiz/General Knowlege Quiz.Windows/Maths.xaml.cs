using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace General_Knowlege_Quiz
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Maths : Page
    {
        private String lastText = "";
        private Boolean isTextFinished = false;
        private Boolean hasStarted_Text = false;
        private Boolean gameStart = false;
        private int corrected_Text = 0;
        private int totalTimeElapsed_Text = 0;
        private int seconds_Text = 0;
        private int totalQuestions_Text = 0;
        private int timeLeft;
        private int guessCount = 1;
        private int newCount = 1;

        private List<Question> questions_Text = new List<Question>();
        private Question currentQuestion = new Question();

        DispatcherTimer timer_Text = new DispatcherTimer();
        DispatcherTimer newTimer = new DispatcherTimer();
        DispatcherTimer guessTimer = new DispatcherTimer();

        public Maths()
        {
            this.InitializeComponent();
            Start();

            newTimer.Interval = TimeSpan.FromSeconds(1);
            // Sub-routine OnTimerTick will be called at every 1 second
            newTimer.Tick += TxtChange_Tick;

            guessTimer.Interval = TimeSpan.FromSeconds(1);
            guessTimer.Tick += New_Tick;

        }

        private void back_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(Games));
        }

        private void TxtChange_Tick(object sender, object e)
        {
            guessCount--;
            //txTimerQuestions.Text = guessCount.ToString();

            if (guessCount == 0)
            {
                AnswersCopy.Text = "";
                newTimer.Stop();
                Answers.Visibility = Visibility.Visible;
            }

        }
        private void New_Tick(object sender, object e)
        {
            newCount--;
            // TimerBlock2.Text = newCount.ToString();

            if (newCount == 1)
            {

            }
            if (newCount == 0)
            {
                Answers.Text = "";
                guessTimer.Stop();

            }

        }

        void messagePromptQuestions_Completed(object sender, EventArgs e)
        {
            Questions_Completed();
        }

        private void Questions_Completed()
        {
            if (questions_Text.Count > 0)
            {
                seconds_Text = 0;
                questions_Text.RemoveAt(0);
                if (questions_Text.Count > 0)
                {
                    this.nextQuestion_Text();
                    timer_Text.Start();

                }
                else
                {
                    this.stopQuestions();
                }
            }
            else
            {
                this.stopQuestions();
            }
        }
        private List<E> ShuffleList<E>(List<E> inputList)
        {
            List<E> randomList = new List<E>();

            Random r = new Random();
            int randomIndex = 0;
            while (inputList.Count > 0)
            {
                randomIndex = r.Next(0, inputList.Count); //Choose a random object in the list
                randomList.Add(inputList[randomIndex]); //add it to the new, random list
                inputList.RemoveAt(randomIndex); //remove to avoid duplicates
            }

            return randomList; //return the new random list
        }

        private void pauseQusetions()
        {
            lastText = currentQuestion.question;
            timer_Text.Stop();

        }

        private void resumeTimer()
        {
            timer_Text.Start();

        }
        void showGameFinished_Completed(object sender, EventArgs e)
        {

            this.restart();
        }




        private void btnAnswerB_Click(object sender, RoutedEventArgs e)
        {
            this.compareQuestions(btnAnswerB.Content.ToString());
        }

        private void btnAnswerA_Click(object sender, RoutedEventArgs e)
        {
            this.compareQuestions(btnAnswerA.Content.ToString());
        }

        private void btnAnswerC_Click(object sender, RoutedEventArgs e)
        {
            this.compareQuestions(btnAnswerC.Content.ToString());
        }

        private void btnAnswerD_Click(object sender, RoutedEventArgs e)
        {
            this.compareQuestions(btnAnswerD.Content.ToString());
        }

        private void nextQuestion_Text()
        {
            questions_Text = ShuffleList<Question>(questions_Text);
            currentQuestion = questions_Text.ElementAt<Question>(0);
            txtQuestionSongs.Text = currentQuestion.text;
            txtTimerQuestions.Text = "Time Left: " + currentQuestion.timer;

            btnAnswerA.Content = currentQuestion.a;
            btnAnswerB.Content = currentQuestion.b;
            btnAnswerC.Content = currentQuestion.c;
            btnAnswerD.Content = currentQuestion.d;
        }

        private void compareQuestions(String answer)
        {
            timer_Text.Stop();
            guessTimer.Start();

            if (answer.Equals(currentQuestion.answer))
            {
                corrected_Text++;
                txtCorrectQuestions.Text = "" + corrected_Text + "/" + totalQuestions_Text;

                Answers.Text = "WOW!!!Correct";

                newCount = 1;
                if (newCount == 0)
                {
                    Answers.Text = "";
                    guessTimer.Stop();
                }
                Questions_Completed();

            }
            else
            {

                // Answers.Text = "Wrong Answer";
                Answers.Visibility = Visibility.Collapsed;
                //AnswersCopy.Text = "Correct Answer is: " + currentQuestion.answer;
                Questions_Completed();

                newTimer.Start();
                guessCount = 1;
                if (guessCount == 0)
                {
                    AnswersCopy.Text = "";
                    newTimer.Stop();
                    Answers.Visibility = Visibility.Visible;
                }

            }

            //if (questions_Text.Count <= 0)
            //{
            //    startGame.IsTapEnabled = false;
            //}
        }

        void timer_Tick_Songs(object sender, object e)
        {
            seconds_Text++;
            timeLeft = currentQuestion.timer - seconds_Text;
            if (timeLeft > 0)
            {
                txtTimerQuestions.Text = "Time Left: " + timeLeft;
            }
            else
            {
                if (questions_Text.Count > 0)
                {
                    seconds_Text = 0;
                    questions_Text.RemoveAt(0);
                    if (questions_Text.Count > 0)
                    {
                        this.nextQuestion_Text();
                    }
                    else
                    {
                        this.stopQuestions();
                    }
                }
                else
                {
                    this.stopQuestions();
                }
            }
        }

        private void stopQuestions()
        {
            seconds_Text = 0;
            timer_Text.Stop();

            isTextFinished = true;

            btnAnswerA.IsEnabled = false;
            btnAnswerB.IsEnabled = false;
            btnAnswerC.IsEnabled = false;
            btnAnswerD.IsEnabled = false;
            txtQuestionSongs.Text = "";

            if (this.isEverythingFinished())
            {
                this.showGameFinished();
            }
        }
        // bool isGameFinished = false;
        private void showGameFinished()
        {
            int totalScore = corrected_Text;
            int totalQuestions = totalQuestions_Text;
            int totalSecondsElapsed = totalTimeElapsed_Text;
            txtTimerQuestions.Text = "";

            Answers.Text = "Game Over!!!Total score: " + totalScore;
            // Answers.Text = "Game Finished with a total score of: " + totalScore + "/" + totalQuestions;
            ScoreBlock.Text = totalScore.ToString();

         
        }

        private void restart()
        {
            lastText = "";
            isTextFinished = false;
            hasStarted_Text = false;
            corrected_Text = 0;
            totalTimeElapsed_Text = 0;
            seconds_Text = 0;
            totalQuestions_Text = 0;
            txtCorrectQuestions.Text = "";
            txtTimerQuestions.Text = "";
            btnAnswerA.IsEnabled = true;
            btnAnswerB.IsEnabled = true;
            btnAnswerC.IsEnabled = true;
            btnAnswerD.IsEnabled = true;
            timer_Text.Stop();
            //timer_Text.Start();
            questions_Text = new List<Question>();
            currentQuestion = new Question();

            this.initialize_Questions();

        }

        private Boolean isEverythingFinished()
        {
            if (isTextFinished)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        //int totalQuestions = 0;
        private void startQuestions()
        {
            // totalQuestions++;
            txtTimerQuestions.Text = "Time Left: " + currentQuestion.timer;
            timer_Text.Start();
            this.nextQuestion_Text();
            hasStarted_Text = true;

        }

        private void initialize_Questions()
        {
            //Q1
            Question question = null;
            question = new Question();
            question.text = "What is the next prime number after 7";
            question.a = "11";
            question.b = "15";
            question.c = "17";
            question.d = "5";
            question.answer = "11";
            question.timer = 10;
            questions_Text.Add(question);

            //Q2
            question = null;
            question = new Question();
            question.text = "The perimeter of a circle is also known as what?";
            question.a = "Pie";
            question.b = "Radius";
            question.d = "The circumference";
            question.c = "Perimeter";
            question.answer = "The circumference";
            question.timer = 10;
            questions_Text.Add(question);

            //Q3
            question = null;
            question = new Question();
            question.text = "65 – 43 = ?";
            question.d = "12";
            question.a = "32";
            question.b = "22";
            question.c = "42";
            question.answer = "22";
            question.timer = 10;
            questions_Text.Add(question);

            //Q4
            question = null;
            question = new Question();
            question.text = "A convex shape curves...?";
            question.a = "Outwards";
            question.d = "Inwards";
            question.b = "Sideways";
            question.c = "None";
            question.answer = "Outwards";
            question.timer = 10;
            questions_Text.Add(question);

            //Q5
            question = null;
            question = new Question();
            question.text = "What does the square root of 144 equal?";
            question.a = "12";
            question.b = "14";
            question.d = "16";
            question.c = "18";
            question.answer = "12";
            question.timer = 10;
            questions_Text.Add(question);

            //Q6
            question = null;
            question = new Question();
            question.text = "Pi can be correctly written as a...";
            question.a = "Whole";
            question.b = "fraction";
            question.d = "Power";
            question.c = "Square root";
            question.answer = "fraction";
            question.timer = 10;
            questions_Text.Add(question);

            //Q7
            question = null;
            question = new Question();
            question.text = "What comes after a million, billion and trillion?";
            question.a = "A million";
            question.b = "A drillion";
            question.d = "A fidrillion";
            question.c = "A quadrillion";
            question.answer = "A quadrillion";
            question.timer = 10;
            questions_Text.Add(question);

            //Q8
            question = null;
            question = new Question();
            question.text = "52 divided by 4 equals what?";
            question.a = "13";
            question.b = "12";
            question.d = "8";
            question.c = "9";
            question.answer = "13";
            question.timer = 10;
            questions_Text.Add(question);

            //Q9
            question = null;
            question = new Question();
            question.text = "What is the bigger number, a googol or a billion or a million or a thousand?";
            question.a = "A googol";
            question.b = "A billion";
            question.d = "A million";
            question.c = "A thousand";
            question.answer = "A googol";
            question.timer = 10;
            questions_Text.Add(question);

            question = null;
            question = new Question();
            question.text = "Opposite angles of a parallelogram are...";
            question.a = "Fixed";
            question.b = "Equal";
            question.d = "Not equal";
            question.c = "Square";
            question.answer = "Equal";
            question.timer = 10;
            questions_Text.Add(question);

            question = null;
            question = new Question();
            question.text = "87 + 56 = ?";
            question.a = "123";
            question.b = "143";
            question.d = "133";
            question.c = "113";
            question.answer = "143";
            question.timer = 10;
            questions_Text.Add(question);

            question = null;
            question = new Question();
            question.text = "How many sides does a nonagon have?";
            question.a = "12";
            question.b = "11";
            question.d = "7";
            question.c = "9";
            question.answer = "9";
            question.timer = 10;
            questions_Text.Add(question);

            question = null;
            question = new Question();
            question.text = "...is an integer.";
            question.a = "-2";
            question.b = " 2/5";
            question.d = "-2.09";
            question.c = "2.50";
            question.answer = "-2";
            question.timer = 10;
            questions_Text.Add(question);

            question = null;
            question = new Question();
            question.text = "What is the next number in the Fibonacci sequence: 0, 1, 1, 2, 3, 5, 8, 13, 21, 34, ?";
            question.a = "47";
            question.b = "55";
            question.d = "49";
            question.c = "51";
            question.answer = "55";
            question.timer = 10;
            questions_Text.Add(question);

            question = null;
            question = new Question();
            question.text = "7 x 9 = ?";
            question.a = "53";
            question.b = "62";
            question.d = "63";
            question.c = "45";
            question.answer = "63";
            question.timer = 10;
            questions_Text.Add(question);

            question = null;
            question = new Question();
            question.text = "In an isosceles triangle all sides are...?";
            question.a = "Equal(2 sides)";
            question.b = "Unequal(2 sides)";
            question.d = "Stretched(2 sides)";
            question.c = "Squared(2 sides)";
            question.answer = "Equal(2 sides)";
            question.timer = 10;
            questions_Text.Add(question);

            question = null;
            question = new Question();
            question.text = "In statistics, the middle value of an ordered set of values is called what?";
            question.a = "The Mean";
            question.b = "Average";
            question.d = "Mode";
            question.c = "The median";
            question.answer = "The median";
            question.timer = 10;
            questions_Text.Add(question);

            question = null;
            question = new Question();
            question.text = "5 to the power of 0 equals what?";
            question.a = "5";
            question.b = "1";
            question.d = "0";
            question.c = "4";
            question.answer = "1";
            question.timer = 10;
            questions_Text.Add(question);

            question = null;
            question = new Question();
            question.text = "What is the area, to nearest whole number, of an equilateral triangle whose perimeter is 300 meters?";
            question.a = "5330 SQ. METERS";
            question.b = "2330 SQ. METERS";
            question.d = "4330 SQ. METERS";
            question.c = "3300 SQ. METERS";
            question.answer = "5330 SQ. METERS";
            question.timer = 10;
            questions_Text.Add(question);

           

            totalQuestions_Text = questions_Text.Count;
            txtCorrectQuestions.Text = "Correct: " + corrected_Text + "/" + totalQuestions_Text;


            timer_Text.Interval = new TimeSpan(0, 0, 0, 1, 0);
            timer_Text.Tick += timer_Tick_Songs;
            hasStarted_Text = false;
            //  questions_Text.Clear();
        }

        private void Start()
        {
            initialize_Questions();
            timer_Text.Stop();
            // totalQuestions = 0;
            seconds_Text = 0;

            if (!hasStarted_Text)
            {
                this.startQuestions();
            }
            else if (!isTextFinished)
            {
                timer_Text.Start();
                //this.resumeTimer();
            }
        }

        private void OnNavigatedTO(object sender, EventArgs e)
        {
            questions_Text.Clear();
        }


      

        void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            ViewHandler();
        }

        
        private void ViewHandler()
        {
            ApplicationView current = ApplicationView.GetForCurrentView();
            if (current.IsFullScreen)
            {
                Snap.Visibility = Visibility.Collapsed;

            }

            else
            {
                Snap.Visibility = Visibility.Visible;

            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Window.Current.SizeChanged += Current_SizeChanged;

        } 
         
    }
}
