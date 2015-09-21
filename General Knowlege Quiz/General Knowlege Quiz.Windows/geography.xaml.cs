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
    public sealed partial class geography : Page
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

        public geography()
        {
            this.InitializeComponent();

            Start();

            newTimer.Interval = TimeSpan.FromSeconds(1);
            // Sub-routine OnTimerTick will be called at every 1 second
            newTimer.Tick += TxtChange_Tick;

            guessTimer.Interval = TimeSpan.FromSeconds(1);
            guessTimer.Tick += New_Tick;
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
            question.text = "What country is also known as Persia?";
            question.a = "Iran";
            question.b = "Italy";
            question.c = "Syria";
            question.d = "Israel";
            question.answer = "Iran";
            question.timer = 10;
            questions_Text.Add(question);

            //Q2
            question = null;
            question = new Question();
            question.text = "In what country would you find Mount Kilimanjaro?";
            question.a = "Tanzania";
            question.b = "Kenya";
            question.d = "Malawi";
            question.c = "Congo";
            question.answer = "Tanzania";
            question.timer = 10;
            questions_Text.Add(question);

            //Q3
            question = null;
            question = new Question();
            question.text = "What major river flows through the Grand Canyon?";
            question.d = "Brandywine River";
            question.a = "Mississippi River";
            question.b = "Hudson River";
            question.c = "Colorado River";
            question.answer = "Colorado River";
            question.timer = 10;
            questions_Text.Add(question);

            //Q4
            question = null;
            question = new Question();
            question.text = "What name is given to the NorthEast part of China?";
            question.a = "Xinjiang";
            question.d = "Manchuria";
            question.b = "Shaanxi";
            question.c = "Shangai";
            question.answer = "Manchuria";
            question.timer = 10;
            questions_Text.Add(question);

            //Q5
            question = null;
            question = new Question();
            question.text = "What is the main Island of Japan?";
            question.a = "Hokkaido";
            question.b = "Kyushu";
            question.d = "Shikoku";
            question.c = "Honshu";
            question.answer = "Honshu";
            question.timer = 10;
            questions_Text.Add(question);

            //Q6
            question = null;
            question = new Question();
            question.text = "In what city is Brandenburg Gate located?";
            question.a = "Paris";
            question.b = "Berlin";
            question.d = "London";
            question.c = "Pretoria";
            question.answer = "Berlin";
            question.timer = 10;
            questions_Text.Add(question);

            //Q7
            question = null;
            question = new Question();
            question.text = "In what country would you find the city of Limerick?";
            question.a = "USA";
            question.b = "Italy";
            question.d = "UK";
            question.c = "Ireland";
            question.answer = "Ireland";
            question.timer = 10;
            questions_Text.Add(question);

            //Q8
            question = null;
            question = new Question();
            question.text = "What well-known mountain pass connects Pakistan and Afghanistan?";
            question.a = "Durand Pass";
            question.b = "Afghan Pass";
            question.d = "Pakhtunkhwa Pass";
            question.c = "Khyber Pass";
            question.answer = "Khyber Pass";
            question.timer = 10;
            questions_Text.Add(question);

            //Q9
            question = null;
            question = new Question();
            question.text = "What volcano exploded n 1883 with the force of 200 megatons of TNT?";
            question.a = "Krakatoa";
            question.b = "Mount Pinatubo";
            question.d = "Guarapuava";
            question.c = "Santa Maria";
            question.answer = "Krakatoa";
            question.timer = 10;
            questions_Text.Add(question);

            question = null;
            question = new Question();
            question.text = "Old Trafford stadium is located near what major city?";
            question.a = "New York";
            question.b = "Manchester";
            question.d = "Liverpool";
            question.c = "London";
            question.answer = "Manchester";
            question.timer = 10;
            questions_Text.Add(question);

            question = null;
            question = new Question();
            question.text = "Ceylon is the former name of what city?";
            question.a = "New Delhi";
            question.b = "London";
            question.d = "Paris";
            question.c = "Sri Lanka";
            question.answer = "Sri Lanka";
            question.timer = 10;
            questions_Text.Add(question);

            question = null;
            question = new Question();
            question.text = "What city was once called New Amsterdam?";
            question.a = "Amsterdam";
            question.b = "New York City";
            question.d = "New Jersey";
            question.c = "Toronto";
            question.answer = "New York City";
            question.timer = 10;
            questions_Text.Add(question);

            question = null;
            question = new Question();
            question.text = "Albania and Serbia are located onwhat peninsula?";
            question.a = "Balkan";
            question.b = "Iberian";
            question.d = "Gydan";
            question.c = "Crimean";
            question.answer = "Balkan";
            question.timer = 10;
            questions_Text.Add(question);

            question = null;
            question = new Question();
            question.text = "What is Pakistan's longest river?";
            question.a = "Lyari River";
            question.b = "Chenab River";
            question.d = "Indus River";
            question.c = "Basol River";
            question.answer = "Indus River";
            question.timer = 10;
            questions_Text.Add(question);

            question = null;
            question = new Question();
            question.text = "What strait divides Morocco and Spain?";
            question.a = "Starit of Gilbraltar";
            question.b = "Strait of Bonifacio";
            question.d = "Bali Strait";
            question.c = "Strait of Belle Isle";
            question.answer = "Starit of Gilbraltar";
            question.timer = 10;
            questions_Text.Add(question);

            question = null;
            question = new Question();
            question.text = "By surface area, which is the largest of Africa's great lakes?";
            question.a = "Lake Chad";
            question.b = "Lake Tanganyika";
            question.d = "Lake Victoria";
            question.c = "Lake Malawi";
            question.answer = "Lake Victoria";
            question.timer = 10;
            questions_Text.Add(question);

            question = null;
            question = new Question();
            question.text = "What country is often described as being shaped like a boot?";
            question.a = "Italy";
            question.b = "Ireland";
            question.d = "South Africa";
            question.c = "German";
            question.answer = "Italy";
            question.timer = 10;
            questions_Text.Add(question);

            question = null;
            question = new Question();
            question.text = "What is the largest Island in the Mediterranean Seas?";
            question.a = "Cyprus";
            question.b = "Majorca";
            question.d = "Sicily";
            question.c = "Ibiza";
            question.answer = "Sicily";
            question.timer = 10;
            questions_Text.Add(question);

            question = null;
            question = new Question();
            question.text = "What canal connects the Red and Mediterranean Seas?";
            question.a = "Grand Canal";
            question.b = "Suez Canal";
            question.d = "Panama Canal";
            question.c = "Saimaa Canal";
            question.answer = "Suez Canal";
            question.timer = 10;
            questions_Text.Add(question);

            question = null;
            question = new Question();
            question.text = "Which gorge in Nepal is deepest in the world?";
            question.a = "Kali Gandaki Gorge";
            question.b = "Chatra Gorge";
            question.d = "Gandikota";
            question.c = "Papi Hills";
            question.answer = "Kali Gandaki Gorge";
            question.timer = 10;
            questions_Text.Add(question);

            question = null;
            question = new Question();
            question.text = "Which country was formerly known as Nyasaland?";
            question.a = "Zimbabwe";
            question.b = "Kenya";
            question.d = "Malawi";
            question.c = "Zambia";
            question.answer = "Malawi";
            question.timer = 10;
            questions_Text.Add(question);

            question = null;
            question = new Question();
            question.text = "In South Africa, the name Soweto is derived from a combination of which English words? ";
            question.a = "South Western Township";
            question.b = "South Westerly Township";
            question.d = "Soweto Township";
            question.c = "Sowerview TOwnship";
            question.answer = "South Westerly Township";
            question.timer = 10;
            questions_Text.Add(question);

            question = null;
            question = new Question();
            question.text = "What covers 85% of Algeria?";
            question.a = "Oil";
            question.b = "Water";
            question.d = "Vegetation";
            question.c = "Sahara Desert";
            question.answer = "Sahara Desert";
            question.timer = 10;
            questions_Text.Add(question);

            question = null;
            question = new Question();
            question.text = "Perth is the capital of which Australian state? ";
            question.a = "Southern Australia";
            question.b = "Eastern Australia";
            question.d = "Western Australia";
            question.c = "Northern Australia";
            question.answer = "Western Australia";
            question.timer = 10;
            questions_Text.Add(question);

            question = null;
            question = new Question();
            question.text = "Which aid organisation's emblem is the Swiss flag with its colours reversed?";
            question.a = "Save the Children";
            question.b = "Red Cross";
            question.d = "World Vision Int";
            question.c = "United Nations";
            question.answer = "Red Cross";
            question.timer = 10;
            questions_Text.Add(question);

            question = null;
            question = new Question();
            question.text = "Which is the largest US state?";
            question.a = "New York";
            question.b = "Texas";
            question.d = "Washington";
            question.c = "Alaska";
            question.answer = "Alaska";
            question.timer = 10;
            questions_Text.Add(question);

            question = null;
            question = new Question();
            question.text = "What does DC stand for in Washington DC?";
            question.a = "District Capital";
            question.b = "District County";
            question.d = "District of California";
            question.c = "District of Columbia";
            question.answer = "District of Columbia";
            question.timer = 10;
            questions_Text.Add(question);

            question = null;
            question = new Question();
            question.text = "Which ocean lies between Europe and America?";
            question.a = "Atlantic";
            question.b = "Indian";
            question.d = "Artic";
            question.c = "Mediterrenean";
            question.answer = "Atlantic";
            question.timer = 10;
            questions_Text.Add(question);

            question = null;
            question = new Question();
            question.text = "What is the Eastern-most country in the EU?";
            question.a = "Poland";
            question.b = "Cyprus";
            question.d = "Croatia";
            question.c = "Slovakia";
            question.answer = "Cyprus";
            question.timer = 10;
            questions_Text.Add(question);

            question = null;
            question = new Question();
            question.text = "How many EU member countries are there as of October 2014?";
            question.a = "27";
            question.b = "28";
            question.d = "30";
            question.c = "29";
            question.answer = "28";
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

        private void back_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Games));
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
