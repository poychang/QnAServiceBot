import {CommonModule} from '@angular/common';
import {HttpClientModule} from '@angular/common/http';
import {ModuleWithProviders, NgModule} from '@angular/core';
import {ReactiveFormsModule} from '@angular/forms';

import {MaterialModule} from './material/material.module';

@NgModule({
  imports: [CommonModule, HttpClientModule, MaterialModule, ReactiveFormsModule],
  exports:
    [CommonModule, HttpClientModule, MaterialModule, ReactiveFormsModule]
})
export class SharedModule {
}
