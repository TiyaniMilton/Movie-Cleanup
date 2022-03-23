using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Movie_Cleanup.Modules
{
    /// <summary>
    /// Provides access to functionality that is common
    /// across all sound players.
    /// </summary>
    /// <seealso cref="IWaveformPlayer"/>
    /// <seealso cref="ISpectrumPlayer"/>
    public interface ISoundPlayer : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets whether the sound player is currently playing audio.
        /// </summary>
        bool IsPlaying { get; }
    }
}
