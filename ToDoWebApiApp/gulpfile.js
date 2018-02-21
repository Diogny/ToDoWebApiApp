"use strict";
var gulp = require("gulp");

var devices = [{
	"type": "desktop",
	"css": {
		"src": "src/desktop/css/",
		"min": "dist/desktop/css/",
		"lib": {
			"dest": "wwwroot/css/",
			"files": [
				"../lib/bootstrap/dist/css/bootstrap.css",
				"../lib/font-awesome/css/font-awesome.css"
			]
		}
	},
	"js": {
		"src": "src/desktop/js/",
		"min": "dist/desktop/js/",
		"lib": {
			"dest": "wwwroot/js/",
			"files": [
				"../lib/jquery/dist/jquery.js",
				"../lib/bootstrap/dist/js/bootstrap.js",
				"../lib/jquery-validation/dist/jquery.validate.js",
				"../lib/jquery-validation/dist/jquery.validate.min.js",
				"../lib/jquery-validation-unobtrusive/jquery.validate.unobtrusive.js",
				"../lib/jquery-validation-unobtrusive/jquery.validate.unobtrusive.min.js"
			]
		}
	},
}
];

gulp.task("copylib", function () {

	devices.forEach(function (dev) {
		var destpath;
		//copy bower lib files
		//js
		if (dev.js.lib && dev.js.lib.files) {
			destpath = dev.js.lib.dest;
			var txt = "\r\n" + dev.js.lib.files.join("\r\n");
			console.log("copying javascript lib files: " + txt);
			//
			gulp.src(dev.js.lib.files)
				.pipe(gulp.dest(destpath));
		}
		//css
		if (dev.css.lib && dev.css.lib.files) {
			destpath = dev.css.lib.dest;
			txt = "\r\n" + dev.css.lib.files.join("\r\n");
			console.log("copying css lib files: " + txt);
			//
			gulp.src(dev.css.lib.files)
				.pipe(gulp.dest(destpath));
		}
	});

});