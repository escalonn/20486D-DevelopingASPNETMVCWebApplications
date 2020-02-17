'use strict';

const gulp = require('gulp');
const concat = require('gulp-concat');
const uglify = require('gulp-uglify');
const sass = require('gulp-sass');
const cssmin = require('gulp-cssmin');

const paths = {
  styles: {
    scssSrc: 'Styles/*.scss',
    dest: 'wwwroot/css/'
  },
  scripts: {
    jqueryJs: 'node_modules/jquery/dist/jquery.js',
    jsSrc: 'Scripts/*.js',
    dest: 'wwwroot/scripts/'
  }
};

function copyJsFile() {
  return gulp.src(paths.scripts.jqueryJs)
    .pipe(gulp.dest(paths.scripts.dest));
}

function minVendorJs() {
  return gulp.src(paths.scripts.jqueryJs)
    .pipe(concat('vendor.min.js'))
    .pipe(uglify())
    .pipe(gulp.dest(paths.scripts.dest));
}

function minJs() {
  return gulp.src(paths.scripts.jsSrc)
    .pipe(concat('script.min.js'))
    .pipe(uglify())
    .pipe(gulp.dest(paths.scripts.dest));
}

function minCss() {
  return gulp.src(paths.styles.scssSrc)
    .pipe(sass().on('error', sass.logError))
    .pipe(concat('main.min.css'))
    .pipe(cssmin())
    .pipe(gulp.dest(paths.styles.dest));
}

function jsWatcher() {
  return gulp.watch(paths.scripts.jsSrc, gulp.series(minJs));
}

function sassWatcher() {
  return gulp.watch(paths.styles.scssSrc, gulp.series(minCss));
}

exports.copyJsFile = copyJsFile;
exports.minVendorJs = minVendorJs;
exports.minJs = minJs;
exports.minCss = minCss;
exports.jsWatcher = jsWatcher;
exports.sassWatcher = sassWatcher;
