'use strict';

const gulp = require('gulp');
const concat = require('gulp-concat');
const uglify = require('gulp-uglify');

const paths = {
  scripts: {
    jquery: 'node_modules/jquery/dist/jquery.js',
    src: 'Scripts/*.js',
    dest: 'wwwroot/scripts/'
  }
};

function copyJsFile() {
  return gulp.src(paths.scripts.jquery)
    .pipe(gulp.dest(paths.scripts.dest));
}

function minVendorJs() {
  return gulp.src(paths.scripts.jquery)
    .pipe(concat('vendor.min.js'))
    .pipe(uglify())
    .pipe(gulp.dest(paths.scripts.dest));
}

function minJs() {
  return gulp.src(paths.scripts.src)
    .pipe(concat('script.min.js'))
    .pipe(uglify())
    .pipe(gulp.dest(paths.scripts.dest));
}

function jsWatcher() {
  return gulp.watch(paths.scripts.src, gulp.series(minJs));
}

exports.copyJsFile = copyJsFile;
exports.minVendorJs = minVendorJs;
exports.minJs = minJs;
exports.jsWatcher = jsWatcher;
