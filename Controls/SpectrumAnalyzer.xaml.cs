using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Un4seen.Bass;
using System.Windows.Threading;
using System.ComponentModel;
using Un4seen.Bass;
using WPFSoundVisualizationLib;
using Movie_Cleanup.Modules;




namespace Movie_Cleanup.Controls
{
    /// <summary>
    /// Interaction logic for SpectrumAnalyzer.xaml
    /// </summary>
    public partial class SpectrumAnalyzer : UserControl
    {
        //public SpectrumAnalyzer()
        //{
        //    InitializeComponent();
        //}

         #region Fields
         private DispatcherTimer animationTimer;
         private RenderTargetBitmap anaylzerBuffer;
         private DrawingVisual drawingVisual = new DrawingVisual();
         private float[] channelData = new float[2048];
         private float[] channelPeakData;
         private int scaleFactorLinear = 9;
         private int scaleFactorSqr = 2;
         private int maxFFTData = 4096;
         private BASSData maxFFT = (BASSData.BASS_DATA_AVAILABLE | BASSData.BASS_DATA_FFT4096);
         private double bandWidth = 1.0;
         private double barWidth = 1;
         private int maximumFrequencyIndex = 2047;
         private int minimumFrequencyIndex = 0;
         private int sampleFrequency = 44100;
         private int[] barIndexMax;
         private int[] barLogScaleIndexMax;
         //private BassEngine bassEngine;
         private ISpectrumPlayer bassEngine;
         #endregion

        public ISpectrumPlayer BassEngine
         {
             get { return bassEngine; }
             set { bassEngine = value; }
         }

         #region Dependency Property Declarations
         public static readonly DependencyProperty MaximumFrequencyProperty;
         public static readonly DependencyProperty MinimumFrequencyProperty;
         public static readonly DependencyProperty BarCountProperty;
         public static readonly DependencyProperty BarSpacingProperty;
         public static readonly DependencyProperty PeakFallDelayProperty;

         protected static readonly DependencyProperty StreamHandleProperty =
             DependencyProperty.Register("StreamHandle",
             typeof(int),
             typeof(SpectrumAnalyzer));

         public static readonly DependencyProperty FrequencyScaleIsLinearProperty =
             DependencyProperty.Register("FrequencyScaleIsLinear",
             typeof(bool),
             typeof(SpectrumAnalyzer));


         public static readonly DependencyProperty BarHeightScalingProperty =
             DependencyProperty.Register("BarHeightScaling",
             typeof(BarHeightScaling),
             typeof(SpectrumAnalyzer));

         public static readonly DependencyProperty AveragePeaksProperty =
             DependencyProperty.Register("AveragePeaks",
             typeof(bool),
             typeof(SpectrumAnalyzer));

         public static readonly DependencyProperty BarBrushProperty =
             DependencyProperty.Register("BarBrush",
             typeof(Brush),
             typeof(SpectrumAnalyzer));

         public static readonly DependencyProperty PeakBrushProperty =
             DependencyProperty.Register("PeakBrush",
             typeof(Brush),
             typeof(SpectrumAnalyzer));
         #endregion

         #region Dependency Properties
         /// <summary>
         /// The maximum display frequency (right side) for the spectrum analyzer.
         /// </summary>
         /// <remarks>This value should be somewhere between 0 and half of the maximum sample rate. If using
         /// the maximum sample rate, this would be roughly 22000.</remarks>
         [Category("Common")]
         public int MaximumFrequency
         {
             get { return (int)GetValue(MaximumFrequencyProperty); }
             set
             {
                 SetValue(MaximumFrequencyProperty, value);
             }
         }

         /// <summary>
         /// The minimum display frequency (left side) for the spectrum analyzer.
         /// </summary>
         [Category("Common")]
         public int MinimumFrequency
         {
             get { return (int)GetValue(MinimumFrequencyProperty); }
             set
             {
                 SetValue(MinimumFrequencyProperty, value);
             }
         }

         /// <summary>
         /// The number of bars to show on the sprectrum analyzer.
         /// </summary>
         /// <remarks>A bar's width can be a minimum of 1 pixel. If the BarSpacing and BarCount property result
         /// in the bars being wider than the chart itself, the BarCount will automatically scale down.</remarks>
         [Category("Common")]
         public int BarCount
         {
             get { return (int)GetValue(BarCountProperty); }
             set
             {
                 SetValue(BarCountProperty, value);
             }
         }

         /// <summary>
         /// The brush used to paint the bars on the spectrum analyzer.
         /// </summary>
         [Category("Common")]
         public Brush BarBrush
         {
             get { return (Brush)GetValue(BarBrushProperty); }
             set
             {
                 SetValue(BarBrushProperty, value);
             }
         }

         /// <summary>
         /// The brush used to paint the peaks on the spectrum analyzer.
         /// </summary>
         [Category("Common")]
         public Brush PeakBrush
         {
             get { return (Brush)GetValue(PeakBrushProperty); }
             set
             {
                 SetValue(PeakBrushProperty, value);
             }
         }

         /// <summary>
         /// The delay factor for the peaks falling. This is relative to the
         /// refresh rate of the chart.
         /// </summary>
         [Category("Common")]
         public int PeakFallDelay
         {
             get { return (int)GetValue(PeakFallDelayProperty); }
             set
             {
                 SetValue(PeakFallDelayProperty, value);
             }
         }

         /// <summary>
         /// The spacing, in pixels, between the bars.
         /// </summary>
         [Category("Common")]
         public double BarSpacing
         {
             get { return (double)GetValue(BarSpacingProperty); }
             set
             {
                 SetValue(BarSpacingProperty, value);
             }
         }

         /// <summary>
         /// If true, the bar height will be displayed linearly with the intensity value.
         /// Otherwise, the bars will be scaled with a square root function.
         /// </summary>
         [Category("Common")]
         public BarHeightScaling BarHeightScaling
         {
             get { return (BarHeightScaling)GetValue(BarHeightScalingProperty); }
             set
             {
                 SetValue(BarHeightScalingProperty, value);
             }
         }

         /// <summary>
         /// If true, this will display the frequency scale (X-axis of the spectrum analyzer)
         /// in a linear scale. Otherwise, the scale will be logrithmic.
         /// </summary>
         [Category("Common")]
         public bool FrequencyScaleIsLinear
         {
             get { return (bool)GetValue(FrequencyScaleIsLinearProperty); }
             set
             {
                 SetValue(FrequencyScaleIsLinearProperty, value);
             }
         }

         /// <summary>
         /// If true, each bar's peak value will be averaged with the previous
         /// bar's peak. This creates a smoothing effect on the bars.
         /// </summary>
         [Category("Common")]
         public bool AveragePeaks
         {
             get { return (bool)GetValue(AveragePeaksProperty); }
             set
             {
                 SetValue(AveragePeaksProperty, value);
             }
         }

         protected int StreamHandle
         {
             get { return (int)GetValue(StreamHandleProperty); }
             set
             {
                 SetValue(StreamHandleProperty, value);
                 if (StreamHandle != 0)
                 {
                     BASS_CHANNELINFO info = new BASS_CHANNELINFO();
                     Bass.BASS_ChannelGetInfo(StreamHandle, info);
                     sampleFrequency = info.freq;
                 }
                 else
                 {
                     sampleFrequency = 44100;
                 }
                 BarMappingChanged(this, EventArgs.Empty);
             }
         }
         #endregion

         #region Dependency Property Validation
         private static object CoerceMaximumFrequency(DependencyObject d, object value)
         {
             SpectrumAnalyzer spectrumAnalyzer = (SpectrumAnalyzer)d;
             if ((int)value < spectrumAnalyzer.MinimumFrequency)
                 return spectrumAnalyzer.MinimumFrequency + 1;
             return value;
         }

         private static object CoerceMinimumFrequency(DependencyObject d, object value)
         {
             int returnValue = (int)value;
             SpectrumAnalyzer spectrumAnalyzer = (SpectrumAnalyzer)d;
             if (returnValue < 0)
                 return returnValue = 0;
             spectrumAnalyzer.CoerceValue(MaximumFrequencyProperty);
             return returnValue;
         }

         private static object CoerceBarCount(DependencyObject d, object value)
         {
             int returnValue = (int)value;
             returnValue = Math.Max(returnValue, 1);
             return returnValue;
         }

         private static object CoercePeakFallDelay(DependencyObject d, object value)
         {
             int returnValue = (int)value;
             returnValue = Math.Max(returnValue, 0);
             return returnValue;
         }

         private static object CoerceBarSpacing(DependencyObject d, object value)
         {
             double returnValue = (double)value;
             returnValue = Math.Max(returnValue, 0);
             return returnValue;
         }
         #endregion

         #region Constructors
         static SpectrumAnalyzer()
         {
             // MaximumFrequency
             FrameworkPropertyMetadata maximumFrequencyMetadata = new FrameworkPropertyMetadata(20000);
             maximumFrequencyMetadata.CoerceValueCallback = new CoerceValueCallback(SpectrumAnalyzer.CoerceMaximumFrequency);
             MaximumFrequencyProperty = DependencyProperty.Register("MaximumFrequency", typeof(int), typeof(SpectrumAnalyzer), maximumFrequencyMetadata);

             // MinimumFrequency
             FrameworkPropertyMetadata minimumFrequencyMetadata = new FrameworkPropertyMetadata(0);
             minimumFrequencyMetadata.CoerceValueCallback = new CoerceValueCallback(SpectrumAnalyzer.CoerceMinimumFrequency);
             MinimumFrequencyProperty = DependencyProperty.Register("MinimumFrequency", typeof(int), typeof(SpectrumAnalyzer), minimumFrequencyMetadata);

             // BarCount
             FrameworkPropertyMetadata barCountMetadata = new FrameworkPropertyMetadata(24);
             barCountMetadata.CoerceValueCallback = new CoerceValueCallback(SpectrumAnalyzer.CoerceBarCount);
             BarCountProperty = DependencyProperty.Register("BarCount", typeof(int), typeof(SpectrumAnalyzer), barCountMetadata);

             // BarSpacing
             FrameworkPropertyMetadata barSpacingMetadata = new FrameworkPropertyMetadata(5.0);
             barSpacingMetadata.CoerceValueCallback = new CoerceValueCallback(SpectrumAnalyzer.CoerceBarSpacing);
             BarSpacingProperty = DependencyProperty.Register("BarSpacing", typeof(double), typeof(SpectrumAnalyzer), barSpacingMetadata);

             // PeakFallDelay
             FrameworkPropertyMetadata peakFallDelayMetadata = new FrameworkPropertyMetadata(5);
             peakFallDelayMetadata.CoerceValueCallback = new CoerceValueCallback(SpectrumAnalyzer.CoercePeakFallDelay);
             PeakFallDelayProperty = DependencyProperty.Register("PeakFallDelay", typeof(int), typeof(SpectrumAnalyzer), peakFallDelayMetadata);
         }

         public SpectrumAnalyzer()
         {
             PeakBrush = new SolidColorBrush(Colors.GreenYellow);
             BarBrush = new LinearGradientBrush(Colors.ForestGreen, Colors.DarkGreen, new Point(0, 1), new Point(0, 0));

             InitializeComponent();

             animationTimer = new DispatcherTimer(DispatcherPriority.ApplicationIdle);
             animationTimer.Interval = TimeSpan.FromMilliseconds(25);
             animationTimer.Tick += new EventHandler(animationTimer_Tick);

             DependencyPropertyDescriptor backgroundDescriptor = DependencyPropertyDescriptor.FromProperty(BackgroundProperty, typeof(SpectrumAnalyzer));
             backgroundDescriptor.AddValueChanged(this, AppearanceChanged);
             DependencyPropertyDescriptor barBrushDescriptor = DependencyPropertyDescriptor.FromProperty(BarBrushProperty, typeof(SpectrumAnalyzer));
             barBrushDescriptor.AddValueChanged(this, AppearanceChanged);

             DependencyPropertyDescriptor maxFrequencyDescriptor = DependencyPropertyDescriptor.FromProperty(MaximumFrequencyProperty, typeof(SpectrumAnalyzer));
             maxFrequencyDescriptor.AddValueChanged(this, BarMappingChanged);
             DependencyPropertyDescriptor minFrequencyDescriptor = DependencyPropertyDescriptor.FromProperty(MinimumFrequencyProperty, typeof(SpectrumAnalyzer));
             maxFrequencyDescriptor.AddValueChanged(this, BarMappingChanged);
             DependencyPropertyDescriptor barCountDescriptor = DependencyPropertyDescriptor.FromProperty(BarCountProperty, typeof(SpectrumAnalyzer));
             maxFrequencyDescriptor.AddValueChanged(this, BarMappingChanged);

             BarMappingChanged(this, EventArgs.Empty);

             if (!DesignerProperties.GetIsInDesignMode(this))
             {
                 //bassEngine = BassEngine.Instance;
                 Bass.BASS_StreamFree(StreamHandle);
                 //UIHelper.Bind(bassEngine, "ActiveStreamHandle", this, StreamHandleProperty);
                // animationTimer.Start();
             }
         }
         #region Public Methods
         /// <summary>
         /// Register a sound player from which the spectrum analyzer
         /// can get the necessary playback data.
         /// </summary>
         /// <param name="soundPlayer">A sound player that provides spectrum data through the ISpectrumPlayer interface methods.</param>
         public void registerSoundPlayer(ref ISpectrumPlayer soundPlayer)
         {
             bassEngine = soundPlayer;
             soundPlayer.PropertyChanged += soundPlayer_PropertyChanged;
             //UpdateBarLayout();
             //animationTimer.Start();
         }
         #endregion

        //private void soundPlayer_PropertyChanged()
        // {
        //     animationTimer.Start();
        // }

         private void soundPlayer_PropertyChanged(object sender, PropertyChangedEventArgs e)
         {
             switch (e.PropertyName)
             {
                 case "IsPlaying":
                     if (bassEngine.IsPlaying && !animationTimer.IsEnabled)
                         animationTimer.Start();
                     break;
             }
         }
         void animationTimer_Tick(object sender, EventArgs e)
         {
             UpdateSpectrum();
         }
         #endregion

         #region Event Overrides
         protected override void OnRender(DrawingContext dc)
         {
             base.OnRender(dc);
             anaylzerBuffer = new RenderTargetBitmap((int)RenderSize.Width, (int)RenderSize.Height, 96, 96, PixelFormats.Pbgra32);
             if (SpectrumImage != null)
             {
                 SpectrumImage.Source = anaylzerBuffer;
             }
             UpdateSpectrum();
         }

         protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
         {
             base.OnRenderSizeChanged(sizeInfo);
             BarMappingChanged(this, EventArgs.Empty);
         }
         #endregion

         #region Private Utility Methods
         private void UpdateSpectrum()
         {
             if (bassEngine == null || drawingVisual == null || anaylzerBuffer == null || RenderSize.Width < 1 || RenderSize.Height < 1)
                 return;
             //.IsPlaying == false)//
             ////if (!bassEngine.IsPaused && (StreamHandle == 0 || (GetFFTBuffer(StreamHandle, (int)maxFFT) < 1)))
             if (!bassEngine.IsPlaying == false && (StreamHandle == 0 || (GetFFTBuffer(StreamHandle, (int)maxFFT) < 1)))
                 return;

             // Clear Canvas
             anaylzerBuffer.Clear();

             using (DrawingContext drawingContext = drawingVisual.RenderOpen())
             {
                 // Draw background if applicable.    
                 if (Background != null)
                     drawingContext.DrawRectangle(Background, null, new Rect(0, 0, RenderSize.Width, RenderSize.Height));

                 // Draw Spectrum Lines
                 RenderSpectrumLines(drawingContext);
             }

             anaylzerBuffer.Render(drawingVisual);
         }

         private int GetFFTBuffer(int handle, int length)
         {
             return Un4seen.Bass.Bass.BASS_ChannelGetData(handle, this.channelData, length);
         }

         private void RenderSpectrumLines(DrawingContext dc)
         {
             double fftBucketHeight = 0f;
             double barHeight = 0f;
             double lastPeakHeight = 0f;
             double peakYPos = 0f;
             double height = this.RenderSize.Height;
             int barIndex = 0;
             double peakDotHeight = Math.Max(barWidth / 2.0f, 1);
             double barHeightScale = (height - peakDotHeight);
             const double minDBValue = -90;
             const double maxDBValue = 0;
             const double dbScale = (maxDBValue - minDBValue);

             for (int i = minimumFrequencyIndex; i < maximumFrequencyIndex; i++)
             {
                 // If we're paused, keep drawing, but set the current height to 0 so the peaks fall.
                 if (bassEngine.IsPlaying == false)//IsPaused)
                 {
                     barHeight = 0f;
                 }
                 else // Draw the maximum value for the bar's band
                 {
                     switch (BarHeightScaling)
                     {
                         case BarHeightScaling.Decibel:
                             double dbValue = 20 * Math.Log10((double)channelData[i]);
                             fftBucketHeight = ((dbValue - minDBValue) / dbScale) * barHeightScale;
                             break;
                         case BarHeightScaling.Linear:
                             fftBucketHeight = (channelData[i] * scaleFactorLinear) * barHeightScale;
                             break;
                         case BarHeightScaling.Sqrt:
                             fftBucketHeight = (((Math.Sqrt((double)this.channelData[i])) * scaleFactorSqr) * barHeightScale);
                             break;
                     }

                     if (barHeight < fftBucketHeight)
                         barHeight = fftBucketHeight;
                     if (barHeight < 0f)
                         barHeight = 0f;
                 }

                 // If this is the last FFT bucket in the bar's group, draw the bar.
                 int currentIndexMax = FrequencyScaleIsLinear ? barIndexMax[barIndex] : barLogScaleIndexMax[barIndex];
                 if (i == currentIndexMax)
                 {
                     // Peaks can't surpass the height of the control.
                     if (barHeight > height)
                         barHeight = height;

                     if (AveragePeaks && barIndex > 0)
                         barHeight = (lastPeakHeight + barHeight) / 2;

                     peakYPos = barHeight;

                     if (channelPeakData[barIndex] < peakYPos)
                         this.channelPeakData[barIndex] = (float)peakYPos;
                     else
                         this.channelPeakData[barIndex] = (float)(peakYPos + (PeakFallDelay * this.channelPeakData[barIndex])) / ((float)(PeakFallDelay + 1));

                     double xCoord = BarSpacing + (barWidth * barIndex) + (BarSpacing * barIndex) + 1;

                     // Draw the bars
                     if (BarBrush != null)
                         dc.DrawRectangle(BarBrush, null, new Rect(xCoord, (height - 1) - barHeight, barWidth, barHeight));

                     // Draw the peaks
                     if (PeakBrush != null)
                         dc.DrawRectangle(PeakBrush, null, new Rect(xCoord, (height - 1) - this.channelPeakData[barIndex], barWidth, peakDotHeight));

                     lastPeakHeight = barHeight;
                     barHeight = 0f;
                     barIndex++;
                 }
             }
         }
         #endregion

         #region Dependency Property Changed Handlers
         private void AppearanceChanged(object sender, EventArgs e)
         {
             UpdateSpectrum();
         }

         private void BarMappingChanged(object sender, EventArgs e)
         {
             barWidth = Math.Max((int)((RenderSize.Width - (BarSpacing * (BarCount + 1))) / (double)BarCount), 1);
             maximumFrequencyIndex = Math.Min(Utils.FFTFrequency2Index(MaximumFrequency, maxFFTData, sampleFrequency) + 1, 2047);
             minimumFrequencyIndex = Math.Min(Utils.FFTFrequency2Index(MinimumFrequency, maxFFTData, sampleFrequency), 2047);
             bandWidth = Math.Max(((double)(maximumFrequencyIndex - minimumFrequencyIndex)) / RenderSize.Width, 1.0);

             int actualBarCount = Math.Max((int)((RenderSize.Width - BarSpacing) / (barWidth + BarSpacing)), 1);
             channelPeakData = new float[actualBarCount];

             int indexCount = maximumFrequencyIndex - minimumFrequencyIndex;
             int linearIndexBucketSize = (int)Math.Round((double)indexCount / (double)actualBarCount, 0);
             List<int> maxIndexList = new List<int>();
             List<int> maxLogScaleIndexList = new List<int>();
             double maxLog = Math.Log(actualBarCount, actualBarCount);
             for (int i = 1; i < actualBarCount - 1; i++)
             {
                 maxIndexList.Add(minimumFrequencyIndex + (i * linearIndexBucketSize));
                 int logIndex = (int)((maxLog - Math.Log(actualBarCount - i, actualBarCount)) * indexCount) + minimumFrequencyIndex;
                 maxLogScaleIndexList.Add(logIndex);
             }
             maxIndexList.Add(maximumFrequencyIndex);
             maxLogScaleIndexList.Add(maximumFrequencyIndex);
             barIndexMax = maxIndexList.ToArray();
             barLogScaleIndexMax = maxLogScaleIndexList.ToArray();
         }
         #endregion

         internal void RegisterSoundPlayer(ISpectrumPlayer musicplayer)
         {
             ISpectrumPlayer spc = musicplayer;
             this.registerSoundPlayer(ref spc);
         }

         public void soundPlayer_PropertyChanged(int _StreamHandle)
         {
             StreamHandle = _StreamHandle;
             //Bass.BASS_StreamFree(StreamHandle);
             //if (bassEngine.IsPlaying)// && !animationTimer.IsEnabled)
             animationTimer.Start();
         }
    }

     public enum BarHeightScaling
     {
         Decibel,
         Sqrt,
         Linear
     }

    
}
