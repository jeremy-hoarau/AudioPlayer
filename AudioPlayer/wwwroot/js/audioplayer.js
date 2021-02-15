var wavesurfer;

var lastSongIndex;
var currentSongIndex;
var songsPaths;
var songsTitles;

$(document).ready(function () {

    wavesurfer = WaveSurfer.create({
        container: '#waveform'
    });

    wavesurfer.on('ready', function () {
        wavesurfer.play();
    });

    LoadSong();
});


function LoadSong() {
    wavesurfer.load(window.location.protocol + "//" + window.location.host + "/" + songsPaths[currentSongIndex]);
    document.getElementById("song-title").textContent = songsTitles[currentSongIndex];
}

function PlayPause() {
    wavesurfer.playPause();
}

function ToggleMute() {
    wavesurfer.toggleMute();
}

function PreviousSong() {
    currentSongIndex = (currentSongIndex > 0 ? currentSongIndex-1 : lastSongIndex);
    LoadSong();
}

function NextSong() {
    currentSongIndex = (currentSongIndex == lastSongIndex ? 0 : currentSongIndex + 1);
    LoadSong();
}