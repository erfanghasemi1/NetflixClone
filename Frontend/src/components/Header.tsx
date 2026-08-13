import React from 'react';

interface HeaderProps {
  isLoading?: boolean;
}

export const Header: React.FC<HeaderProps> = ({ isLoading = false }) => {
  return (
    <header className="sticky top-0 z-50 bg-gradient-to-r from-slate-900 via-purple-900 to-slate-900 shadow-xl">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-4">
        <div className="flex items-center justify-between">
          <div className="flex items-center gap-3">
            <div className="flex items-center justify-center w-10 h-10 bg-gradient-to-br from-purple-400 to-pink-500 rounded-lg shadow-lg">
              <svg className="w-6 h-6 text-white" fill="currentColor" viewBox="0 0 20 20">
                <path d="M2 6a2 2 0 012-2h12a2 2 0 012 2v8a2 2 0 01-2 2H4a2 2 0 01-2-2V6zm4 2v4h8V8H6z" />
              </svg>
            </div>
            <div>
              <h1 className="text-2xl font-bold text-white">MovieFinder</h1>
              <p className="text-xs text-purple-200">Discover your next favorite film</p>
            </div>
          </div>
          {isLoading && (
            <div className="flex items-center gap-2">
              <div className="animate-spin">
                <svg className="w-5 h-5 text-purple-400" fill="none" viewBox="0 0 24 24">
                  <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4" />
                  <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z" />
                </svg>
              </div>
              <span className="text-purple-200 text-sm">Loading data...</span>
            </div>
          )}
        </div>
      </div>
    </header>
  );
};
