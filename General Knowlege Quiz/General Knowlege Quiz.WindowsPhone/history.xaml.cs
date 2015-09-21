using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace General_Knowlege_Quiz
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class history : Page
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
        int newCount = 1;

        private List<Question> questions_Text = new List<Question>();
        private Question currentQuestion = new Question();

        DispatcherTimer timer_Text = new DispatcherTimer();
        DispatcherTimer newTimer = new DispatcherTimer();
        DispatcherTimer guessTimer = new DispatcherTimer();

        public history()
        {
            this.InitializeComponent();

            Start();

            newTimer.Interval = TimeSpan.FromSeconds(1);
            // Sub-routine OnTimerTick will be called at every 1 second
            newTimer.Tick += TxtChange_Tick;

            guessTimer.Interval = TimeSpan.FromSeconds(1);
            guessTimer.Tick += New_Tick;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
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

            Answers.Text = "Game Finished with a total score of:";
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
            question.text = "The first Olympiad was held in Greece in the year";
            question.a = "776 BC";
            question.b = "750 BC";
            question.c = "600 BC";
            question.d = "760 BC";
            question.answer = "776 BC";
            question.timer = 10;
            questions_Text.Add(question);

            //Q2
            question = null;
            question = new Question();
            question.text = "Rome was founded in the year";
            question.a = "753 BC";
            question.b = "200 BC";
            question.d = "750 BC";
            question.c = "650 BC";
            question.answer = "753 BC";
            question.timer = 10;
            questions_Text.Add(question);

            //Q3
            question = null;
            question = new Question();
            question.text = "The Great wall of China was built in the year";
            question.d = "214 BC";
            question.a = "200 BC";
            question.b = "100 BC";
            question.c = "250 BC";
            question.answer = "214 BC";
            question.timer = 10;
            questions_Text.Add(question);

            //Q4
            question = null;
            question = new Question();
            question.text = "The first voyage around the world was undertook by whom in 1522";
            question.a = "Vasco de Gama";
            question.d = "Magellan";
            question.b = "Christopher Columbus";
            question.c = "Bartolomeu Dias";
            question.answer = "Magellan";
            question.timer = 10;
            questions_Text.Add(question);

            //Q5
            question = null;
            question = new Question();
            question.text = "The first President of USA was?";
            question.a = "John Adams";
            question.b = "George Washington";
            question.d = "James Madison";
            question.c = "Thomas Jefferson";
            question.answer = "George Washington";
            question.timer = 10;
            questions_Text.Add(question);

            //Q6
            question = null;
            question = new Question();
            question.text = "Which Battle marked the end of Napoleon era?";
            question.a = "Aboukir";
            question.b = "Amstetten";
            question.d = "Barrosa";
            question.c = "Waterloo";
            question.answer = "Waterloo";
            question.timer = 10;
            questions_Text.Add(question);

            //Q7
            question = null;
            question = new Question();
            question.text = "The American War of Independence was fought between?";
            question.a = "America and Canada";
            question.b = "America and Japan";
            question.d = "America and Great Britain";
            question.c = "America and German";
            question.answer = "America and Great Britain";
            question.timer = 10;
            questions_Text.Add(question);

            //Q8
            question = null;
            question = new Question();
            question.text = "Adolf Hitler was also known as?";
            question.a = "Fuhrer";
            question.b = "Adolf";
            question.d = "Htler";
            question.c = "Dictator";
            question.answer = "Fuhrer";
            question.timer = 10;
            questions_Text.Add(question);

            //Q9
            question = null;
            question = new Question();
            question.text = "The first woman in world to become the Prime minister of a country was?";
            question.a = "Indira Gandhi";
            question.b = "Golda Meir";
            question.d = "Sirimao Bandara Naike";
            question.c = "Margaret Thatcher";
            question.answer = "Sirimao Bandara Naike";
            question.timer = 10;
            questions_Text.Add(question);

            question = null;
            question = new Question();
            question.text = "The Emperor of Germany who dismissed his Chancellor Bismark in 1980 was?";
            question.a = "William I";
            question.b = "William II";
            question.d = "William IV";
            question.c = "Henry V";
            question.answer = "William II";
            question.timer = 10;
            questions_Text.Add(question);

            question = null;
            question = new Question();
            question.text = "Earliest two Presidents of USA were father and son.Their names were?";
            question.a = "John Adams & Quincey";
            question.b = "George Bush";
            question.d = "John & George Kennedy";
            question.c = "Theodore & Franklin Roosevelt";
            question.answer = "John Adams & Quincey";
            question.timer = 10;
            questions_Text.Add(question);

            question = null;
            question = new Question();
            question.text = "The king of England before Elizabeth II was?";
            question.a = "George VI";
            question.b = "George III";
            question.d = "George II";
            question.c = "George V";
            question.answer = "George VI";
            question.timer = 10;
            questions_Text.Add(question);

            question = null;
            question = new Question();
            question.text = "The Statue of Liberty of New York was a gift from?";
            question.a = "France";
            question.b = " Britain";
            question.d = "Ireland";
            question.c = "German";
            question.answer = "France";
            question.timer = 10;
            questions_Text.Add(question);

            question = null;
            question = new Question();
            question.text = "Hitler's secret service was also known as?";
            question.a = "SAS";
            question.b = "Gestapo";
            question.d = "CIA";
            question.c = "Polizei";
            question.answer = "Gestapo";
            question.timer = 10;
            questions_Text.Add(question);

            question = null;
            question = new Question();
            question.text = "The last Emperor of Rome was?";
            question.a = "Augustus Caesar";
            question.b = "Julius Caesar";
            question.d = "Romulus Augustus";
            question.c = "Mark Antony";
            question.answer = "Romulus Augustus";
            question.timer = 10;
            questions_Text.Add(question);

            question = null;
            question = new Question();
            question.text = "The first World War ended in?";
            question.a = "1918";
            question.b = "1945";
            question.d = "1921";
            question.c = "1915";
            question.answer = "1918";
            question.timer = 10;
            questions_Text.Add(question);

            question = null;
            question = new Question();
            question.text = "The Second World War ended in?";
            question.a = "1946";
            question.b = "1936";
            question.d = "1940";
            question.c = "1945";
            question.answer = "1945";
            question.timer = 10;
            questions_Text.Add(question);

            question = null;
            question = new Question();
            question.text = "The first Prime Minister of Independent Kenya was?";
            question.a = "Daniel Arap Moi";
            question.b = "Jomo Kenyatta";
            question.d = "Kenyatta Uhuru";
            question.c = "Mwai Kibaki";
            question.answer = "Jomo Kenyatta";
            question.timer = 10;
            questions_Text.Add(question);

            question = null;
            question = new Question();
            question.text = "The first British Prime Minister was?";
            question.a = "Earl of Wilmington";
            question.b = "Henry Pelham";
            question.d = "Sir Robert Walpole";
            question.c = "John Major";
            question.answer = "Sir Robert Walpole";
            question.timer = 10;
            questions_Text.Add(question);

            question = null;
            question = new Question();
            question.text = "In the Second World War,atomic bombs were dropped in Japan which destroyed?";
            question.a = "Nagasaki";
            question.b = "Hiroshima";
            question.d = "Tokyo";
            question.c = "Yokohama";
            question.answer = "Hiroshima";
            question.timer = 10;
            questions_Text.Add(question);

            question = null;
            question = new Question();
            question.text = "Hitler launched Operation Barbarossa in 1941 against the?";
            question.a = "Jews";
            question.b = "Polish";
            question.d = "France";
            question.c = "Soviet Union";
            question.answer = "Soviet Union";
            question.timer = 10;
            questions_Text.Add(question);

            question = null;
            question = new Question();
            question.text = "Christopher Columbus died in the year?";
            question.a = "1506 AD";
            question.b = "1500 AD";
            question.d = "1450 AD";
            question.c = "1516 AD";
            question.answer = "1506 AD";
            question.timer = 10;
            questions_Text.Add(question);

            question = null;
            question = new Question();
            question.text = "The Disease which struck Europe in the 14th Century was?";
            question.a = "Bird Flu";
            question.b = "Cholera";
            question.d = "Plague";
            question.c = "Hepatitis";
            question.answer = "Plague";
            question.timer = 10;
            questions_Text.Add(question);

            question = null;
            question = new Question();
            question.text = "In 1867,USA purchased Alaska from? ";
            question.a = "Canada";
            question.b = "Britain";
            question.d = "Spain";
            question.c = "Russia";
            question.answer = "Russia";
            question.timer = 10;
            questions_Text.Add(question);

            question = null;
            question = new Question();
            question.text = "In 1818,the Zulu Kingdom was founded by?";
            question.a = "Dingiswayo";
            question.b = "Shaka";
            question.d = "Dutch";
            question.c = "Dingane";
            question.answer = "Shaka";
            question.timer = 10;
            questions_Text.Add(question);

            question = null;
            question = new Question();
            question.text = "Benito Mussolini was the dictator of?";
            question.a = "German";
            question.b = "France";
            question.d = "Italy";
            question.c = "Russia";
            question.answer = "Italy";
            question.timer = 10;
            questions_Text.Add(question);

            question = null;
            question = new Question();
            question.text = "The European Economic Community was formed in?";
            question.a = "1958";
            question.b = "2001";
            question.d = "1980";
            question.c = "2005";
            question.answer = "1958";
            question.timer = 10;
            questions_Text.Add(question);

            question = null;
            question = new Question();
            question.text = "The popular leader of Cuba is?";
            question.a = "King Solomon";
            question.b = "King David";
            question.d = "Nebuchadnezzar";
            question.c = "Fidel Castro";
            question.answer = "Fidel Castro";
            question.timer = 10;
            questions_Text.Add(question);

            question = null;
            question = new Question();
            question.text = "Before independence Ghana was a colony of? ";
            question.a = "France";
            question.b = "German";
            question.d = "Britain";
            question.c = "Italy";
            question.answer = "Britain";
            question.timer = 10;
            questions_Text.Add(question);

            question = null;
            question = new Question();
            question.text = "In December 1941,Japan attacked?";
            question.a = "Florida";
            question.b = "Pearl Harbour";
            question.d = "Washington DC";
            question.c = "New York";
            question.answer = "Pearl Harbour";
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

        private void btnAnswerA_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.compareQuestions(btnAnswerA.Content.ToString());
        }

        private void btnAnswerB_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.compareQuestions(btnAnswerB.Content.ToString());
        }

        private void btnAnswerC_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.compareQuestions(btnAnswerC.Content.ToString());
        }

        private void btnAnswerD_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.compareQuestions(btnAnswerD.Content.ToString());
        }

        private void back_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(games));
        }

        /*

        private void chkScores_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SoreQuiz));
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
         */
    }
}
