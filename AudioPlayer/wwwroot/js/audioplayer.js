var wavesurfer;

$(document).ready(function () {

    wavesurfer = WaveSurfer.create({
        container: '#waveform'
    });

    wavesurfer.on('ready', function () {
        wavesurfer.play();
        console.log("auto play");
    });
});


function LoadSong(songPath) {
    wavesurfer.load(window.location.protocol + "//" + window.location.host + "/" + songPath);
    console.log("load song");
}