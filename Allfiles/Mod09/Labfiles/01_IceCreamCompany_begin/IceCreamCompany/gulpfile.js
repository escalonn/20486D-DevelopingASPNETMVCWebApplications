'use strict';

const gulp = require('gulp');
const concat = require('gulp-concat');
const uglify = require('gulp-uglify');
const sass = require('gulp-sass');
const cssmin = require('gulp-cssmin');

const paths = {
  styles: {
    bootstrap: 'node_modules/bootstrap/dist/css/bootstrap.css',
    scss: 'Styles/*.scss',
    dest: 'wwwroot/css/'
  },
  scripts: {
    jquery: 'node_modules/jquery/dist/jquery.js',
    popperJs: 'node_modules/popper.js/dist/umd/popper.js',
    bootstrap: 'node_modules/bootstrap/dist/js/bootstrap.js',
    js: 'Scripts/*.js',
    dest: 'wwwroot/scripts/'
  }
};

function copyJsFile() {
  return gulp.src(paths.scripts.jquery)
    .pipe(gulp.dest(paths.scripts.dest));
}

function minVendorJs() {
  return gulp.src([
    paths.scripts.jquery,
    paths.scripts.popperJs,
    paths.scripts.bootstrap
  ])
    .pipe(concat('vendor.min.js'))
    .pipe(uglify())
    .pipe(gulp.dest(paths.scripts.dest));
}

function minJs() {
  return gulp.src(paths.scripts.js)
    .pipe(concat('script.min.js'))
    .pipe(uglify())
    .pipe(gulp.dest(paths.scripts.dest));
}

function minScss() {
  return gulp.src(paths.styles.scss)
    .pipe(sass().on('error', sass.logError))
    .pipe(concat('main.min.css'))
    .pipe(cssmin())
    .pipe(gulp.dest(paths.styles.dest));
}

function minVendorCss() {
  return gulp.src(paths.styles.bootstrap)
    .pipe(concat('vendor.min.css'))
    .pipe(cssmin())
    .pipe(gulp.dest(paths.styles.dest));
}

function jsWatcher() {
  return gulp.watch(paths.scripts.js, gulp.series(minJs));
}

function sassWatcher() {
  return gulp.watch(paths.styles.scss, gulp.series(minScss));
}

exports.copyJsFile = copyJsFile;
exports.minVendorJs = minVendorJs;
exports.minJs = minJs;
exports.minScss = minScss;
exports.minVendorCss = minVendorCss;
exports.jsWatcher = jsWatcher;
exports.sassWatcher = sassWatcher;
