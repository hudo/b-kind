'use strict';

var gulp = require("gulp");
var sass = require("gulp-sass");


var paths = {
    webroot: "./wwwroot/",
    node_modules: "./node_modules/"
};

var npm_module = {
    jquery: paths.node_modules + "/jquery/dist/jquery.min.js",
    bootstrap: paths.node_modules + "/bootstrap/dist/js/bootstrap.min.js"
}

gulp.task("sass", function () {
    return gulp.src("./Styles/main.scss")
        .pipe(sass())
        .pipe(gulp.dest('./wwwroot/css'));
});

gulp.task("copy", function () {
    return gulp.src([npm_module.jquery, npm_module.bootstrap])
        .pipe(gulp.dest('./wwwroot/js'));
});

gulp.task("watch", function () {
    gulp.watch("./Styles/**/*.scss", ["sass"]); 

});
